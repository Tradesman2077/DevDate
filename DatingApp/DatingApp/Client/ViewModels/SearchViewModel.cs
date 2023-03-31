using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DatingApp.Client;
using DatingApp.Shared;
using Newtonsoft.Json.Linq;


namespace DatingApp.ViewModels
{
    public class SearchViewModel
    {
        public List<User> Suggestions { get; set; } = new();
        public long Id { get; set; }
        public string City { get; set; }
        private HttpClient _httpClient;
        public SearchViewModel() { }
        public SearchViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public static implicit operator SearchViewModel(User user)
        {
            return new SearchViewModel
            {
                Id = user.Id,
                City = user.City
            };
        }
        public static implicit operator User(SearchViewModel user)
        {
            return new User
            {
                Id = user.Id,
                City = user.City
            };
        }
        public async Task<List<User>> GetSuggestions()
        {
            List<User> suggestions = await _httpClient.GetFromJsonAsync<List<User>>("user/getsuggestions/" + this.Id);

            List<User> sortedSuggestions = await RankSuggestions(suggestions);
            LoadSuggestions(sortedSuggestions);
            return this.Suggestions;
        }
        public async Task Like(long removeId)
        {
            User? user = await _httpClient.GetFromJsonAsync<User>("user/getuser/" + this.Id);
            await _httpClient.PutAsJsonAsync("user/addlike/" + removeId, user);
        }

        private async void LoadSuggestions(List<User> suggestions)
        {
            this.Suggestions = new List<User>();
            foreach (User user in suggestions)
            {
                this.Suggestions.Add(user);
            }
        }
        public async Task GetUser()
        {
            User? user = await _httpClient.GetFromJsonAsync<User>("user/getuser/" + this.Id);
            LoadUser(user);
        }
        private void LoadUser(SearchViewModel searchViewModel)
        {
            this.Id = searchViewModel.Id;
            this.City = searchViewModel.City;
        }
        private async Task<List<User>> RankSuggestions(List<User> suggestions)
        {
            List<User> sortedSuggested = new();
            //get lat long of user 
            var userLatLong = await _httpClient.GetStringAsync($"https://dev.virtualearth.net/REST/v1/Locations/{this.City}/?key=AqRcDES1IjWz478mzEWbyU6PFOoHs-IHfQD8taxwoLQHrCBKU_YXV6ackKqfUP21");
            JObject userJson = JObject.Parse(userLatLong);
            var userPoints = userJson["resourceSets"][0]["resources"][0]["point"]["coordinates"];
            var usrCoordinates = userPoints.ToObject<List<double>>();

            var hashMap = new Dictionary<User, double>();
            //get distance to user for each suggestion
            foreach (var suggestion in suggestions)
            {
                var latLong = await _httpClient.GetStringAsync($"https://dev.virtualearth.net/REST/v1/Locations/{suggestion.City}/?key=AqRcDES1IjWz478mzEWbyU6PFOoHs-IHfQD8taxwoLQHrCBKU_YXV6ackKqfUP21");
                JObject json = JObject.Parse(latLong);
                var points = json["resourceSets"][0]["resources"][0]["point"]["coordinates"];
                var coordinates = points.ToObject<List<double>>();
                var dist = DistanceUtil.Distance(usrCoordinates[0], usrCoordinates[1], coordinates[0], coordinates[1], 'M');
                hashMap.Add(suggestion, dist);
            }
            //order based on dist
            var list = hashMap.ToList();
            list.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            foreach (var item in list)
            {
                System.Console.WriteLine(item.Key.City + ": " + item.Value);
                sortedSuggested.Add(item.Key);
            }
            return sortedSuggested;

        }
    }
}