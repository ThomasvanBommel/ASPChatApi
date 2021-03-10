using System;
using Npgsql;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi2.Controllers{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase{

        /// <summary>
        ///   Create account
        /// </summary>
        /// <remarks>
        ///   Create a new user account
        /// </remarks>
        /// <param name="Username">Desired username</param>
        /// <param name="Password">Plain-text password</param>
        /// <response code="201">Created account</response>
        /// <response code="500">Internal server error</response>
        [HttpPost, Route("create")]
        public Response CreateAccount(string Username, string Password){
            User user = new User(Username, Password);
            NpgsqlException e = Database.AddUser(user);

            if(e != null){
                this.HttpContext.Response.StatusCode = 500;
                return new Response("Error => " + e.Message);
            }

            this.HttpContext.Response.StatusCode = 201;
            return new Response(user);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>
        /// Login if user already exists
        /// </remarks>
        /// <param name="Username">Username of the account</param>
        /// <param name="Password">Plain-text password</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad request, invalid username or password</response>
        [HttpPatch, Route("login")]
        public Response Login(string Username, string Password){
            User user = Database.GetUserByName(Username);

            if(user != null){
                // I forsee a bug here...
                Guid token = user.Login(Password);

                if(token.CompareTo(Guid.Empty) != 0 && Database.SetToken(Username, token) == null){
                    this.HttpContext.Response.StatusCode = 200;
                    return new Response(token);
                }
            }

            this.HttpContext.Response.StatusCode = 400;
            return new Response("Invalid username or password");
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <remarks>
        /// Logins a user out if they are currently logged in, and resets their token. You should
        /// always log out after each session, otherwise a user could post on your behalf,
        /// in the case of a database leak
        /// </remarks>
        /// <param name="Token">User Guid token (see account/login)</param>
        /// <response code="204">No Content</response>
        [HttpPatch, Route("logout")]
        public void Logout(Guid Token){
            Database.RemoveToken(Token);

            this.HttpContext.Response.StatusCode = 204;
        }

    }
}