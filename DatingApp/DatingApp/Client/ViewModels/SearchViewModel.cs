using System.Net.Http.Json;
using DatingApp.Shared;


namespace DatingApp.ViewModels
{
    public class SearchViewModel {
        public List<User> Suggestions { get; set; } = new();
        public long Id { get; set; }
        private HttpClient _httpClient;
        public SearchViewModel(){}
        public SearchViewModel(HttpClient httpClient){
            _httpClient = httpClient;
        }
        public static implicit operator SearchViewModel(User user){
            return new SearchViewModel{
                Id = user.Id
            };
        }
        public static implicit operator User(SearchViewModel user){
            return new User{
                Id = user.Id
            };
        }
        public async Task<List<User>> GetSuggestions()
        {
            List<User> suggestions = await _httpClient.GetFromJsonAsync<List<User>>("user/getsuggestions/" + this.Id);
            LoadSuggestions(suggestions);
            return this.Suggestions;
        }
        public async Task Like(long removeId)
        {
            User? user = await _httpClient.GetFromJsonAsync<User>("user/getuser/" + this.Id);
            await _httpClient.PutAsJsonAsync("user/addlike/" + removeId, user);
            
            GetUser();
            GetSuggestions();
        }

        private async void LoadSuggestions(List<User> suggestions)
        {
            this.Suggestions = new List<User>();
            foreach(User user in suggestions){
                if(user.Id != this.Id){
                    this.Suggestions.Add(user);
                }
            }
        }
        public async Task GetUser()
        {
            User? user = await _httpClient.GetFromJsonAsync<User>("user/getuser/" + this.Id);
            LoadUser(user);
        }
        private void LoadUser(SearchViewModel searchViewModel){
            this.Id = searchViewModel.Id;
        }
    }
}