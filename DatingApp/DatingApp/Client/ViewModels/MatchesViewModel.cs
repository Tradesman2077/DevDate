using System.Net.Http.Json;
using DatingApp.Shared;

namespace DatingApp.ViewModels
{
    public class MatchesViewModel {
        public List<User> Matches { get; set; } = new();
        public long Id { get; set; }
        private HttpClient _httpClient;
        public MatchesViewModel(){}
        public MatchesViewModel(HttpClient httpClient){
            _httpClient = httpClient;
        }
        public static implicit operator MatchesViewModel(User user){
            return new MatchesViewModel{
                Id = user.Id
            };
        }
        public static implicit operator User(MatchesViewModel user){
            return new User{
                Id = user.Id
            };
        }
        public async Task<List<User>> GetMatches()
        {
            List<User> matches = await _httpClient.GetFromJsonAsync<List<User>>("user/getmatches/" + this.Id);
            LoadMatches(matches);
            return this.Matches;
        }
        public async Task RemoveMatch(long removeId)
        {
            User? user = await _httpClient.GetFromJsonAsync<User>("user/getuser/" + this.Id);
            await _httpClient.PutAsJsonAsync("user/removematch/" + removeId, user);
            GetUser();
            GetMatches();
        }

        private async void LoadMatches(List<User> matches)
        {
            this.Matches = new List<User>();
            foreach(User user in matches){
                this.Matches.Add(user);
            }
        }
        public async Task GetUser()
        {
            User? user = await _httpClient.GetFromJsonAsync<User>("user/getuser/" + this.Id);
            LoadUser(user);
        }
        private void LoadUser(MatchesViewModel matchesViewModel){
            this.Id = matchesViewModel.Id;
        }
    }
}