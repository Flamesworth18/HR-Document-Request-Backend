using Document_Request.Data;
using Document_Request.Models.Auth;
using Document_Request.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Document_Request.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public UsersController(DatabaseContext context)
        {
            _context = context;
        }

        #region USER
        //GET all users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.Include(u => u.Requests).ToListAsync();
            if (users == null)
            {
                return NotFound("No users were found");
            }

            return Ok(users);
        }

        //GET user
        [HttpGet("{id:guid}")]
        [ActionName("GetUser")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            var user = await _context.Users.Include(u => u.Requests).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);

        }

        //POST user
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] CreateUpdateUser createUser)
        {
            TokenGenerator.CreatePasswordHash(createUser.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User();
            user.Id = Guid.NewGuid();
            user.FirstName = createUser.FirstName;
            user.LastName = createUser.LastName;
            user.Email = createUser.Email;
            user.Username = createUser.Username;
            user.Sex = createUser.Sex;
            user.Role = createUser.Role;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        //PUT user
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] CreateUpdateUser newUser)
        {
            var oldUser = await _context.Users.Include(u => u.Requests).FirstOrDefaultAsync(u => u.Id == id);
            if (oldUser == null)
            {
                return NotFound("User not found!");
            }

            TokenGenerator.CreatePasswordHash(newUser.Password, out byte[] passwordHash, out byte[] passwordSalt);

            oldUser.FirstName = newUser.FirstName;
            oldUser.LastName = newUser.LastName;
            oldUser.Email = newUser.Email;
            oldUser.Username = newUser.Username;
            oldUser.Sex = newUser.Sex;
            oldUser.Role = newUser.Role;
            oldUser.PasswordHash = passwordHash;
            oldUser.PasswordSalt = passwordSalt;
            await _context.SaveChangesAsync();

            return Ok(oldUser);
        }

        //DELETE user
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found!");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        #endregion

        #region REQUEST

        //GET user requests
        [HttpGet("{id:guid}/requests")]
        public async Task<IActionResult> GetUserRequests([FromRoute] Guid id)
        {
            var user = await _context.Users.Include(u => u.Requests).FirstOrDefaultAsync(u => u.Id == id);
            if(user == null)
            {
                return NotFound("User not found!");
            }

            var userRequests = user.Requests.ToList();
            if(userRequests == null)
            {
                return NotFound("User's requests not found!");
            }

            return Ok(userRequests);
        }

        //POST user request
        [HttpPost("{id:guid}/requests")]
        public async Task<IActionResult> AddUserRequest([FromRoute] Guid id, [FromBody] Request request)
        {
            var user = await _context.Users.Include(u => u.Requests).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound("User not found!");
            }

            request.Id = Guid.NewGuid();
            user.Requests.Add(request);
            user.NumberOfRequests++;

            request.DateCreated = DateTime.UtcNow.ToShortDateString();

            await _context.Requests.AddAsync(request);
            await _context.SaveChangesAsync();

            return Ok(request);
        }

        //PUT user request
        [HttpPut("{id:guid}/requests/{requestId:guid}")]
        public async Task<IActionResult> UpdateUserRequest([FromRoute] Guid id, [FromRoute] Guid requestId, [FromBody] Request newRequest)
        {
            var user = await _context.Users.Include(u => u.Requests).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound("User not found!");
            }

            var userRequest = user.Requests.FirstOrDefault(r => r.Id == requestId);
            if(userRequest == null)
            {
                return NotFound("User requests are not found!");
            }

            userRequest.FirstName = newRequest.FirstName;
            userRequest.LastName = newRequest.LastName;
            userRequest.Purpose = newRequest.Purpose;
            userRequest.Sex = newRequest.Sex;
            userRequest.Rank = newRequest.Rank;
            userRequest.Document = newRequest.Document;
            userRequest.Status = newRequest.Status;

            return Ok(userRequest);
        }

        //DELETE user request
        [HttpDelete("{id:guid}/requests/{requestId:guid}")]
        public async Task<IActionResult> DeleteUserRequest([FromRoute] Guid id, [FromRoute] Guid requestId)
        {
            var user = await _context.Users.Include(u => u.Requests).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound("User not found!");
            }

            var userRequest = user.Requests.FirstOrDefault(r => r.Id == requestId);
            if(userRequest == null)
            {
                return NotFound("User requests are not found!");
            }

            user.Requests.Remove(userRequest);
            _context.Requests.Remove(userRequest);
            await _context.SaveChangesAsync();

            return Ok(userRequest);
        }

        #endregion
    }
}
