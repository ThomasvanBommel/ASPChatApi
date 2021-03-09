using System;
using System.Text;
using System.Security.Cryptography;

namespace ChatApi2{
    public class User{
        public DateTime Joined { get; }
        public string Name { get; }
        public Guid Token { get; set; }
        private string PasswordHash { get; }

        /**
         * Create a new User
         * @param Name - Username for the new user
         * @param RawPassword - Plain text password (will be hashed)
         */
        public User(string Name, string RawPassword){
            Joined = DateTime.Now;
            Token = Guid.Empty;

            this.Name = Name;
            PasswordHash = HashPassword(RawPassword);
        }

        /**
         * Create a new User
         * @param Name - Username for the new user
         * @param Joined - Date joined
         * @param Token - Sign-in uuid token
         * @param RawPassword - Plain text password (will be hashed)
         */
        public User(string Name, DateTime Joined, Guid Token, string PasswordHash){
            this.Name = Name;
            this.Joined = Joined;
            this.Token = Token;
            this.PasswordHash = PasswordHash;
        }

        /**
         * Hash a raw password string into a hash-code
         * @param RawPassword - Password string
         * @returns Passwords hash code
         */
        public string HashPassword(string RawPassword){
            // Yes... I should be using Bcrypt or something better
            return BitConverter.ToString(
                SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(Name + RawPassword))
            );
        }

        /**
         * Login and return the users token
         * @param RawPassword - Password string to check
         * @returns Empty Guid if the login failed, Guid otherwise
         */
        public Guid Login(string RawPassword){
            // Check password
            if(HashPassword(RawPassword) == PasswordHash){

                // Return token if it already exists
                if(Token != Guid.Empty) return Token;

                // Generate new token
                Token = System.Guid.NewGuid();

                // Return the new token
                return Token;
            }

            // Failed login
            return Guid.Empty;
        }

        /**
         * Get this users password hash, I made PasswordHash private so it wouldn't show up
         * in user request responses
         * @returns Hashed password
         */
        public string getHash_(){
            return PasswordHash;
        }

        /**
         * Convert this user into a string
         * For debugging purposes
         * @returns String representation of this user object
         */
        public string ToString(){
            return "User {\n" +
                $"\tName: {Name}\n" +
                $"\tJoined: {Joined}\n" +
                $"\tToken: {Token}\n" +
            "}";
        }
    }
}
