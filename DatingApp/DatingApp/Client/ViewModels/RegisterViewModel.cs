using System.Net.Http.Json;
using DatingApp.Shared;


namespace DatingApp.ViewModels
{
    public class RegisterViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ReenterPassword { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Bio { get; set; }
        public string FavouriteLanguage { get; set; }

        public int Age { get; set; }

        private HttpClient _httpClient;
        public RegisterViewModel()
        {

        }
        public RegisterViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task RegisterUser()
        {
            await _httpClient.PostAsJsonAsync<User>("user/registeruser", this);

        }
        public static implicit operator RegisterViewModel(User user)
        {
            return new RegisterViewModel
            {
                Username = user.Username,
                Password = user.Password,
                ReenterPassword = user.ReenterPassword,
                Email = user.Email,
                Age = user.Age,
                Country = user.Country,
                City = user.City,
                FavouriteLanguage = user.FavouriteLanguage,
                Bio = user.Bio
            };
        }
        public static implicit operator User(RegisterViewModel registerViewModel)
        {
            return new User
            {
                Username = registerViewModel.Username,
                Password = registerViewModel.Password,
                ReenterPassword = registerViewModel.ReenterPassword,
                Email = registerViewModel.Email,
                Age = registerViewModel.Age,
                Country = registerViewModel.Country,
                City = registerViewModel.City,
                FavouriteLanguage = registerViewModel.FavouriteLanguage,
                Bio = registerViewModel.Bio
            };
        }
    }
}