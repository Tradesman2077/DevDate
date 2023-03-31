using System.Net.Http.Json;
using DatingApp.Shared;


namespace DatingApp.ViewModels
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        private HttpClient _httpClient;
        public LoginViewModel()
        {

        }
        public LoginViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task LoginUser()
        {
            await _httpClient.PostAsJsonAsync<User>("user/loginuser", this);
        }
        public static implicit operator LoginViewModel(User user)
        {
            return new LoginViewModel
            {
                Username = user.Username,
                Password = user.Password
            };
        }
        public static implicit operator User(LoginViewModel loginViewModel)
        {
            return new User
            {
                Username = loginViewModel.Username,
                Password = loginViewModel.Password
            };
        }
    }
}