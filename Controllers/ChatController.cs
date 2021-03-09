using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ChatApi2.Controllers{
    [ApiController]
    [Route("chat")]
    public class ChatController : ControllerBase{

        /// <summary>
        /// Submit a message
        /// </summary>
        /// <remarks>
        /// Submit a message for all to see
        /// </remarks>
        /// <param name="Token">GUID user code (see account/login)</param>
        /// <param name="Text">Message to submit</param>
        /// <response code="200">Success or error message</response>
        [HttpPost, Route("send")]
        public Response SendMessage(Guid Token, string Text){
            if(Text == null && Text == "" && Token.CompareTo(Guid.Empty) == 0)
                return new Response("Token or text is empty / missing");

            User user = Database.GetUserByToken(Token);

            if(user != null)
                if(Database.AddMessage(new Message(Text, user.Name)) == null)
                    return new Response("Message submitted successfully!");

            return new Response("Failed to send message. Are you sure this token is valid?");
        }

        /// <summary>
        /// Request submitted messages
        /// </summary>
        /// <remarks>
        /// Request a number of submitted messages from the database
        /// </remarks>
        /// <param name="Token">GUID user code (see account/login)</param>
        /// <param name="Limit">Limit of how many messages to recieve. 0 = All messsages</param>
        /// <param name="Descending">True if you want descending order, false otherwise</param>
        /// <response code="200">Returns a list of messages</response>
        [HttpGet, Route("get")]
        public Response GetMessages(Guid Token, Boolean Descending=true, int Limit=0){
            if(Token.CompareTo(Guid.Empty) != 0)
                if(Database.GetUserByToken(Token) != null)
                    return new Response(Database.GetMessages(Descending, Limit));

            return new Response("Unable to get messages. Please insure you are using a valid token");
        }
    }
}
