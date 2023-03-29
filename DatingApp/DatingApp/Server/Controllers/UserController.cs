using DatingApp.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly DatingappContext _context;

        public UserController(ILogger<UserController> logger, DatingappContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet("getusers")]
        public IEnumerable<User> Get()
        {
                return _context.Users.ToList();
        }
        [HttpGet("getuser/{userid}")]
        public async Task<User> GetUserById(int userId)
        {
            return await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
        }
        [HttpGet("getmatches/{userid}")]
        public async Task<List<User>> GetMatches(int userId)
        {
            List<User>matches = new();
            User currentUserToGetMatches = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

            if(currentUserToGetMatches.Matches.Length == 0)
            {
                return matches;
            }
            var matchArr = currentUserToGetMatches.Matches.Remove(currentUserToGetMatches.Matches.Length - 1).Split(',');

            foreach(string id in matchArr){
                var matchedUser = await _context.Users.Where(u => u.Id.ToString() == id).FirstOrDefaultAsync();
                matches.Add(matchedUser);
            }
            return matches;
        }
        [HttpPut("removematch/{userid}")]
        public async Task<User> RemoveMatch(int userId, [FromBody] User user)
        {
            User userToUpdate = await _context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();
            if(userToUpdate.Matches.Length == 2){
                userToUpdate.Matches = "";
            }
            else{
                List<string> matches = userToUpdate.Matches.Split(',').ToList();
                matches.Remove(userId.ToString());
                string matchString ="";
                foreach(var match in matches)
                {
                    matchString = matchString + match +',';
                }
                userToUpdate.Matches = matchString;
            }
            
            await _context.SaveChangesAsync();

            return await Task.FromResult(user);
        }
        [HttpPut("updateuser/{userid}")]
        public async Task<User> UpdateUser(int userId, [FromBody] User user)
        {
            User userToUpdate = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            userToUpdate.Username = user.Username;
            userToUpdate.City = user.City;
            userToUpdate.FavouriteLanguage = user.FavouriteLanguage;
            userToUpdate.PhotoUrl = user.PhotoUrl;
            await _context.SaveChangesAsync();

            return await Task.FromResult(user);
        }
        ///AUTH
        [HttpPost("loginuser")]
        public async Task<ActionResult<User>> LoginUser(User user)
        {
            var encrytedPassword = Utility.Encrypt(user.Password);
            user.Password = encrytedPassword;
            
            User loggedInUser = await _context.Users.Where(u => u.Username == user.Username && u.Password == user.Password).FirstOrDefaultAsync();

            if (loggedInUser != null)
            {
                //create a claim
                var claim = new Claim(ClaimTypes.Name, loggedInUser.Username);
                //create claimsIdentity
                var claimsIdentity = new ClaimsIdentity(new[] { claim }, "serverAuth");
                //create claimsPrincipal
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                //Sign In User
                await HttpContext.SignInAsync(claimsPrincipal);
            }
            return await Task.FromResult(loggedInUser);
        }
        [HttpGet("getcurrentuser")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            User currentUser = new();
            if(User.Identity.IsAuthenticated){
                var userName = User.FindFirstValue(ClaimTypes.Name);
                currentUser = await _context.Users.Where(u => u.Username == userName).FirstOrDefaultAsync();
            }
            return await Task.FromResult(currentUser);
        }
        [HttpPost("registeruser")]
        public async Task<ActionResult<User>> RegisterUser([FromBody] User newUser)
        {
           var encrytedPassword = Utility.Encrypt(newUser.Password);
           User user = new();

           user.Password = encrytedPassword;
           user.Email = newUser.Email;
           user.Username = newUser.Username;
           user.Bio = newUser.Bio;
           user.Country = newUser.Country;
           user.City = newUser.City;
           user.FavouriteLanguage = newUser.FavouriteLanguage;
           
           await _context.Users.AddAsync(user);
           await _context.SaveChangesAsync();

           return await Task.FromResult(user);
        }
        [HttpGet("logoutuser")]
        public async Task<ActionResult<String>> LogOutUser()
        {
            await HttpContext.SignOutAsync();
            return "Success";
        } 
    }
}