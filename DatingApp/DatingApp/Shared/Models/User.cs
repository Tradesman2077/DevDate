using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Shared
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Bio { get; set; }
        public string FavouriteLanguage { get; set; }
        public string Matches { get; set; } = "";
        public string Likes { get; set; } = "";
        public string Country { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
        public string ReenterPassword { get; set; }
        public List<User> MatchedUsers { get; set; }

        public User()
        {

        }
        public User(string username, string password, string bio, string favouriteLanguage, string country, string city, int age, string email, string reenterPassword)
        {
            Username = username;
            Password = password;
            Bio = bio;
            FavouriteLanguage = favouriteLanguage;
            Country = country;
            City = city;
            Age = age;
            Email = email;
            Matches = Matches;
            ReenterPassword = reenterPassword;
        }
        
    }
    

}
