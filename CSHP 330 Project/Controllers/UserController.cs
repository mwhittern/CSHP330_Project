using CSHP_330_Project.model;
using CSHP_330_Project.Repo;
using Microsoft.AspNetCore.Mvc;

namespace CSHP_330_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authenticator]
    public class UserController : ControllerBase
    {
//        private static List<User> users = new List<User>()
//            {
//                new User(101, "blah@email.com", "pass1"),
//                new User(102, "foo@email.com", "pass2"),
//                new User(103, "bar@email.com", "pass3"),
//            };

        private readonly IUserRepository users;

        public UserController(IUserRepository userRepository)
        {
            users = userRepository;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = users.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound(id);
            }

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post([FromBody] User value)
        {
            if( string.IsNullOrEmpty(value.email) || string.IsNullOrEmpty(value.password))
            {
                return BadRequest();
            }

            int userID = users.Add(value);

            // value.Id = users.Users.Select(u => u.Id).Max() + 1;
            // value.createdDate = DateTime.UtcNow;
            // users.Users.Add(value);

            return CreatedAtAction(nameof(Get), new {id = userID},  value);
        }
        
        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var rowsDeletedFromDb = users.DeleteId(id);// users.RemoveAll(u => u.Id == id);

            if (rowsDeletedFromDb == 0)
            {
                return NotFound(id);
            }
            
            return Ok();
        }
        
        
        [HttpPut]
        public IActionResult Put([FromBody] User value)
        {
            if( string.IsNullOrEmpty(value.email) || value.Id == null)
            {
                return BadRequest();
            }

            var updatedUser = users.Update(value.Id, value);

            if (updatedUser == null)
                return NotFound();
            
            return Ok();
        }
    }
}
