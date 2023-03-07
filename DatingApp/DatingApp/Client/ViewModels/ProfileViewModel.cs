using System.Net.Http.Json;
using DatingApp.Shared;

namespace DatingApp.ViewModels
{
    public class ProfileViewModel {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FavouriteLanguage { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        private HttpClient _httpClient;
        public ProfileViewModel(){

        }
        public ProfileViewModel(HttpClient httpClient){
            _httpClient = httpClient;
        }
        public static implicit operator ProfileViewModel(User user){
            return new ProfileViewModel{
                Username = user.Username,
                Email = user.Email,
                FavouriteLanguage = user.FavouriteLanguage,
                City = user.City,
                Country = user.Country,
                Id = user.Id
            };
        }
        public static implicit operator User(ProfileViewModel user){
            return new User{
                Username = user.Username,
                Email = user.Email,
                FavouriteLanguage = user.FavouriteLanguage,
                City = user.City,
                Country = user.Country,
                Id = user.Id
            };
        }

        public async Task UpdateUser()
        {
            User user = this;

            await _httpClient.PutAsJsonAsync("user/updateuser/" + this.Id, user);
        }
        public async Task GetUser()
        {
            System.Console.WriteLine(this.Id);
            User? user = await _httpClient.GetFromJsonAsync<User>("user/getuser/" + this.Id);
            LoadUser(user);
        }
        private void LoadUser(ProfileViewModel profileViewModel){
            this.Username = profileViewModel.Username;
            this.City = profileViewModel.City;
            this.FavouriteLanguage = profileViewModel.FavouriteLanguage;
        }
    }
    
}