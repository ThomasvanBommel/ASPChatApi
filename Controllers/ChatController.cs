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
        /// <response code="201">Created message successfully</response>
        /// <response code="400">Bad request, token is invalid or text is empty / missing</response>
        [HttpPost, Route("send")]
        public Response SendMessage(Guid Token, string Text){
            if(Text != null && Text != "" && Token.CompareTo(Guid.Empty) != 0){
                User user = Database.GetUserByToken(Token);

                if(user != null){
                    if(Database.AddMessage(new Message(Text, user.Name)) == null){
                        this.HttpContext.Response.StatusCode = 201;
                        return new Response("Message submitted successfully!");
                    }
                }
            }

            this.HttpContext.Response.StatusCode = 400;
            return new Response("Token is invalid or text is empty / missing");
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
        /// <response code="200">Successful</response>
        /// <response code="400">Bad request, token is invalid</response>
        [HttpGet, Route("get")]
        public Response GetMessages(Guid Token, Boolean Descending=true, int Limit=0){
            if(Token.CompareTo(Guid.Empty) != 0){
                if(Database.GetUserByToken(Token) != null){
                    this.HttpContext.Response.StatusCode = 200;
                    return new Response(Database.GetMessages(Descending, Limit));
                }
            }

            this.HttpContext.Response.StatusCode = 400;
            return new Response("Bad request, token is invalid");
        }
    }
}
