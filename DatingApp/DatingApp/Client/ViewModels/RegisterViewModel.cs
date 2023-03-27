using System.Net.Http.Json;
using DatingApp.Shared;


namespace DatingApp.ViewModels
{
    public class RegisterViewModel {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ReenterPassword { get; set; }

        private HttpClient _httpClient;
        public RegisterViewModel()
        {
            
        }
        public RegisterViewModel(HttpClient httpClient){
            _httpClient = httpClient;
        }

        public async Task RegisterUser()
        {   
            await _httpClient.PostAsJsonAsync<User>("user/registeruser", this);
            
        }
        public static implicit operator RegisterViewModel(User user )
        {
            return new RegisterViewModel
            {
                Username = user.Username,
                Password = user.Password,
                ReenterPassword = user.ReenterPassword
            };
        }
        public static implicit operator User(RegisterViewModel registerViewModel )
        {
            return new User
            {
                Username = registerViewModel.Username,
                Password = registerViewModel.Password,
                ReenterPassword = registerViewModel.ReenterPassword
            };
        }
    }
}