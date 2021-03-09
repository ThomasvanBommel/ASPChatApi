using System;
using Npgsql;
using System.Diagnostics;
using System.Collections.Generic;

namespace ChatApi2{
    public class Database{
        static string ConnectionString = 
        "Host=169.254.47.156;Username=postgres;Password=P0st911;Database=chatapi;Pooling=true;";

        /** ACCOUNT ----------------------------------------------------------------------------- */

        /**
         * Add a new user to the database
         * @param Usr - User to add
         * @returns null if successful, NpgsqlException otherwise
         */
        public static NpgsqlException AddUser(User Usr){
            return NonQuery(CreateCommand(
                "INSERT INTO users (name, joined, token, password) "+
                $"VALUES (@name, @joined, null, @hash)",
                new Dictionary<string, object> {
                    { "name", Usr.Name },
                    { "joined", Usr.Joined },
                    { "hash", Usr.getHash_() }
                }
            ));
        }

        /**
         * Get user from the database by username
         * @param Name - Username of the user
         * @returns User if successful, null otherwise
         */
        public static User GetUserByName(string Name){
            return GetUserBy("name", Name);
        }

        /**
         * Get user from the database by token
         * @param Token - Token of the user
         * @returns User if successful, null otherwise
         */
        public static User GetUserByToken(Guid Token){
            if(Token.CompareTo(Guid.Empty) == 0)
                return null;

            return GetUserBy("token", Token.ToString());
        }

        /**
         * Get user by key / value pair
         * @param Key - Key / parameter to check
         * @param Value - Value to check
         * @returns - User if successful, null otherwise
         */
        private static User GetUserBy(string Key, object Value){
            User user = null;

            try{
                var command = CreateCommand(
                    "SELECT name, joined, token, password FROM users WHERE " + Key + " = @value",
                    new Dictionary<string, object> {
                        { "value", Value }
                    }
                );

                var reader = command.ExecuteReader();

                while(reader.Read()){
                    user = new User(
                        reader.GetString(0),
                        DateTime.Parse(reader.GetString(1)),
                        reader.IsDBNull(2) ? Guid.Empty : Guid.Parse(reader.GetString(2)),
                        reader.GetString(3)
                    );
                }

                command.Connection.Close();
            }catch(NpgsqlException e){
                Debug.WriteLine(e.Message);
            }

            return user;
        }

        /**
         * Set database token to null
         * @param Token - Token to check for
         * @returns null if successful, NpgsqlException otherwise
         */
        public static NpgsqlException RemoveToken(Guid Token){
            return NonQuery(CreateCommand(
                "UPDATE users SET token = null WHERE token = @token",
                new Dictionary<string, object> {
                    { "token", Token.ToString() }
                }
            ));
        }

        /**
         * Set user token
         * @param Username - Username of the user you would like to modify
         * @param Token - Token to set
         * @returns null if successful, NpgsqlException otherwise
         */
        public static NpgsqlException SetToken(string Username, Guid Token){
            return NonQuery(CreateCommand(
                "UPDATE users SET token = @token WHERE name = @name",
                new Dictionary<string, object> {
                    { "name", Username },
                    { "token", Token.ToString() }
                }
            ));
        }

        /** CHAT -------------------------------------------------------------------------------- */

        /**
         * Add a message to the database
         * @param Msg - Message to add
         * @returns null if successful, NpgsqlException otherwise
         */
        public static NpgsqlException AddMessage(Message Msg){
            return NonQuery(CreateCommand(
                "INSERT INTO messages (created, text, username) VALUES (@created, @text, @user)",
                new Dictionary<string, object> {
                    { "created", Msg.Created },
                    { "text", Msg.Text },
                    { "user", Msg.Username },
                }
            ));
        }

        /**
         * Get messages from the database
         * @param Decending - false if you want descending data, true otherwise
         * @param Limit - Limit the amount of results, 0=All 
         * @returns List of messages requested
         */
        public static List<Message> GetMessages(Boolean Decending=true, int Limit=0){
            List<Message> messages = new List<Message>();

            try{        
                string limit = Limit > 0 ? $" LIMIT {Limit}" : "";
                string descending = Decending ? "DESC" : "ASC";

                var command = CreateCommand(
                    $"SELECT id, created, text, username FROM messages ORDER BY id {descending}" + 
                    limit,
                    null
                );

                var reader = command.ExecuteReader();

                while(reader.Read()){
                    messages.Add(
                        new Message(
                            reader.GetInt64(0),
                            DateTime.Parse(reader.GetString(1)),
                            reader.GetString(2),
                            reader.GetString(3)
                        )
                    );
                }

                command.Connection.Close();
            }catch(NpgsqlException e){
                Debug.WriteLine(e.Message);
            }

            return messages;
        }

        /** BACKEND ----------------------------------------------------------------------------- */

        /**
         * Execute anything except a SELECT query
         * @param Command - Command to execute
         * @returns null if successful, NpgsqlException otherwise
         */
        private static NpgsqlException NonQuery(NpgsqlCommand Command){
            try{
                Command.ExecuteNonQuery();
                Command.Connection.Close();
            }catch(NpgsqlException e){
                return e;
            }

            return null;
        }

        /**
         * Get a connection to the database
         * @returns Open connection to the database
         */
        private static NpgsqlConnection GetConnection(){
            // Probably inefficient, but I couldn't find in the docs how request a pool connection
            NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);

            connection.Open();

            return connection;
        }

        /**
         * Create an SQL command to be executed on the database
         * @param Command - SQL query string, see https://www.npgsql.org/doc/basic-usage.html
         * @param Parameters - SQL query string parameters, see link above
         * @returns NpgsqlCommand command object
         */
        private static NpgsqlCommand CreateCommand(string Command, Dictionary<string, object> Parameters){
            var command = new NpgsqlCommand(Command, GetConnection());

            if(Parameters != null)
                foreach(var entry in Parameters)
                    command.Parameters.AddWithValue(entry.Key, entry.Value);

            return command;
        }

        /**
         * Read a NpgsqlDataReader response to the debug console
         * @param Reader - Thing to read
         */
        private static void ReadToDebug(NpgsqlDataReader Reader){
            while(Reader.Read()){
                for(int i = 0; i < Reader.FieldCount; i++){
                    string str = Reader.IsDBNull(i) ? "[null]" : Reader.GetString(i);

                    Debug.Write(str + "\t");
                }

                Debug.WriteLine("");
            }
        }
    }
}
