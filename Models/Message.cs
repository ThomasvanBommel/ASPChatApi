using System;

namespace ChatApi2{
    public class Message{
        public Int64 ID { get; }
        public DateTime Created { get; }
        public string Text { get; }
        public string Username { get; }

        public Message(string Text, string Username){
            Created = DateTime.Now;

            this.Text = Text;
            this.Username = Username;
        }

        public Message(Int64 ID, DateTime Created, string Text, string Username){
            this.ID = ID;
            this.Created = Created;
            this.Text = Text;
            this.Username = Username;
        }

    
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
