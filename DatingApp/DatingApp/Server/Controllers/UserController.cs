using DatingApp.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

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
        [HttpGet("getsuggestions/{userid}")]
        public async Task<List<User>> GetSuggestions(int userId)
        {
            List<User> suggestions = new();
            User currentUserToGetSuggestions = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            var matchList = Utility.GetStringAsList(currentUserToGetSuggestions.Matches);
            var likeList = Utility.GetStringAsList(currentUserToGetSuggestions.Likes);
            var users = _context.Users.ToList().Where(x => x.Id != userId);

            foreach (var usr in users)
            {
                if (!matchList.Contains(usr.Id.ToString()) && !likeList.Contains(usr.Id.ToString()))
                {
                    suggestions.Add(usr);
                }
            }
            return suggestions;
        }
        [HttpGet("getuser/{userid}")]
        public async Task<User> GetUserById(int userId)
        {
            return await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
        }
        [HttpGet("getmatches/{userid}")]
        public async Task<List<User>> GetMatches(int userId)
        {
            List<User> matches = new();
            User currentUserToGetMatches = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

            if (currentUserToGetMatches.Matches.Length == 0)
            {
                return matches;
            }
            var matchArr = Utility.GetStringAsList(currentUserToGetMatches.Matches);
            foreach (string id in matchArr)
            {
                var matchedUser = await _context.Users.Where(u => u.Id.ToString() == id).FirstOrDefaultAsync();
                matches.Add(matchedUser);
            }
            return matches;
        }
        [HttpPut("removematch/{userid}")]
        public async Task<User> RemoveMatch(int userId, [FromBody] User user)
        {
            User userToUpdate = await _context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();
            User userToUnlike = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            //remove from matches
            var matches = Utility.GetStringAsList(userToUpdate.Matches);
            matches.Remove(userId.ToString());
            string matchString = "";
            foreach (var match in matches)
            {
                matchString = matchString + match + ',';
            }
            userToUpdate.Matches = matchString;
            //remove from other users matches
            var otherUserMatches = Utility.GetStringAsList(userToUnlike.Matches);
            otherUserMatches.Remove(user.Id.ToString());
            string matchStringg = "";
            foreach (var match in otherUserMatches)
            {
                matchStringg = matchStringg + match + ',';
            }
            userToUnlike.Matches = matchStringg;

            //remove from likes
            var likes = Utility.GetStringAsList(userToUpdate.Likes);
            likes.Remove(userId.ToString());
            string likeString = "";
            foreach (var like in likes)
            {
                likeString = likeString + like + ',';
            }
            userToUpdate.Likes = likeString;

            await _context.SaveChangesAsync();
            return await Task.FromResult(user);
        }
        [HttpPut("addlike/{userid}")]
        public async Task<User> AddLike(int userId, [FromBody] User user)
        {
            //add liked user to current users likes
            User userToUpdate = await _context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();
            User likedUser = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

            var likes = Utility.GetStringAsList(userToUpdate.Likes);

            string likeString = "";
            likes.Add(userId.ToString());
            foreach (var like in likes)
            {
                likeString = likeString + like + ',';
            }
            userToUpdate.Likes = likeString;

            //check if other user has them in their likes also
            var likedUserLikes = Utility.GetStringAsList(likedUser.Likes);


            if (likedUserLikes.Contains(user.Id.ToString()))
            {
                // add match to both users
                likedUser.Matches = likedUser.Matches + user.Id + ",";
                userToUpdate.Matches = userToUpdate.Matches + userId + ",";
                await _context.SaveChangesAsync();
                return await Task.FromResult(user);
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
            if (User.Identity.IsAuthenticated)
            {
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
            user.Matches = "";
            user.Likes = "";

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