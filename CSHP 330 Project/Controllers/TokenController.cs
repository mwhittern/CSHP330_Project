using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using CSHP_330_Project.Controllers;
using CSHP_330_Project.Repo;

namespace HelloWorldService.Controllers
{
    public class TokenRequest
    {
        public string UserEmail { get; set; }
        public string Password { get; set; }
    }

    [Route("login/")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserRepository users;

        public TokenController(IUserRepository userRepository)
        {
            users = userRepository;
        }

        // This should require SSL
        [HttpPost]
        public dynamic Post([FromBody] TokenRequest tokenRequest)
        {
            int userCount = users.Users.Count(u =>
                u.password == tokenRequest.Password &&
                u.email.Trim().ToLower() == tokenRequest.UserEmail.Trim().ToLower());

            if (userCount <= 0)
            {
                return Unauthorized();
            }
            
            var token = TokenHelper.GetToken(tokenRequest.UserEmail, tokenRequest.Password);
            return new { Token = token };
        }

        // This should require SSL
        [HttpGet("{userEmail}/{password}")]
        public dynamic Get(string userEmail, string password)
        {
            int userCount = users.Users.Count(u =>
                u.password == password &&
                u.email.Trim().ToLower() == userEmail.Trim().ToLower());

            if (userCount <= 0)
            {
                return Unauthorized();
            }
            
            var token = TokenHelper.GetToken(userEmail, password);
            return new { Token = token };
        }
    }
}