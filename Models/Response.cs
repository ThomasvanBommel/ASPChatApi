using System;

namespace ChatApi2{
    public class Response{
        public DateTime Date { get; }
        public object Object { get; }

        /**
         * Create a new response object
         * @param Object - Data to send in response
         */
        public Response(object Object){
            Date = DateTime.Now;

            this.Object = Object;
        }
    }
}
