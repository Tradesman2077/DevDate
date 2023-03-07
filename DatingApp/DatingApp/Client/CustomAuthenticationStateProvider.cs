
using System.Net.Http.Json;
using System.Security.Claims;

using DatingApp.Client.Models;

using Microsoft.AspNetCore.Components.Authorization;

namespace DatingApp.Client
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {

        private readonly HttpClient _httpClient;

        public CustomAuthenticationStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            User currentUser = await _httpClient.GetFromJsonAsync<User>("user/getcurrentuser");
            if (currentUser != null && currentUser.Username != null)
            { 
                 //create a claim
                var claimUsername = new Claim(ClaimTypes.Name, currentUser.Username);
                var claimNameIdentifier = new Claim(ClaimTypes.NameIdentifier, Convert.ToString(currentUser.Id));

                //create claimsIdentity
                var claimsIdentity = new ClaimsIdentity(new[] { claimUsername , claimNameIdentifier}, "serverAuth");
                //create claimsPrincipal
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                return new AuthenticationState(claimsPrincipal);
            }
            else
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
} 