using System.Net.Http.Json;
using DatingApp.Shared;

namespace DatingApp.ViewModels
{
    public class MatchesViewModel {
        public List<User> Matches { get; set; }

        private HttpClient _httpClient;
        public MatchesViewModel(){

        }
        public MatchesViewModel(HttpClient httpClient){
            _httpClient = httpClient;
        }

        public async Task GetMatches()
        {
            List<User> matches = await _httpClient.GetFromJsonAsync<List<User>>("user/getmatches/1");
            LoadMatches(matches);
        }
        

        private async void LoadMatches(List<User> matches)
        {
            this.Matches = new List<User>();
            foreach(User user in matches){
                this.Matches.Add(user);
            }
        }
    }
}