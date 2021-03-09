using System;

namespace ChatApi2{
    public class Response{
        public DateTime Date { get; }
        public object Object { get; }

        public Response(object Object){
            Date = DateTime.Now;

            this.Object = Object;
        }
    }
}
