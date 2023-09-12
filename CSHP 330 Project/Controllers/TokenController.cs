using Microsoft.AspNetCore.Mvc;
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
        private readonly IUserRepository _users;

        public TokenController(IUserRepository userRepository)
        {
            _users = userRepository;
        }

        // This should require SSL
        [HttpPost]
        public dynamic Post([FromBody] TokenRequest tokenRequest)
        {
            int userCount = _users.Users.Count(u =>
                u.password == tokenRequest.Password &&
                string.Equals(u.email.Trim(), tokenRequest.UserEmail.Trim(), StringComparison.CurrentCultureIgnoreCase));

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
            int userCount = _users.Users.Count(u =>
                u.password == password &&
                string.Equals(u.email.Trim(), userEmail.Trim(), StringComparison.CurrentCultureIgnoreCase));

            if (userCount <= 0)
            {
                return Unauthorized();
            }
            
            var token = TokenHelper.GetToken(userEmail, password);
            return new { Token = token };
        }
    }
}