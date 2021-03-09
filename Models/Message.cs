using System;

namespace ChatApi2{
    public class Message{
        public Int64 ID { get; }
        public DateTime Created { get; }
        public string Text { get; }
        public string Username { get; }

        /**
         * Create a new message object
         * @param Text - Actual message
         * @param Username - User that created the message
         */
        public Message(string Text, string Username){
            Created = DateTime.Now;

            this.Text = Text;
            this.Username = Username;
        }

        /**
         * Create an existing message (for database response)
         * @param ID - Message ID in database
         * @param Created - What date/time the message was created
         * @param Text - Actual message
         * @param Username - User that sent the message
         */
        public Message(Int64 ID, DateTime Created, string Text, string Username){
            this.ID = ID;
            this.Created = Created;
            this.Text = Text;
            this.Username = Username;
        }

        /**
         * Convert this message into a string
         * For debugging purposes
         * @returns String representation of this message object
         */
        public string ToString(){
            return "Message {\n" +
                $"\tID: {ID}\n" +
                $"\tCreated: {Created}\n" +
                $"\tText: {Text}\n" +
                $"\tUsername: {Username}\n" +
            "}";
        }
    }
}
