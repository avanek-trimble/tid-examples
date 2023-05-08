using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Blazor.Server.CustomAuthorization
{
    /// <summary>
    /// Custom authorization handler to inspect the TID access token, and attempt to get the TID user's email
    /// address. The Email address is then checked against an allow-listed set of email address domains.
    /// </summary>
    public class EmailDomainClaimHandler : AuthorizationHandler<EmailDomainRequirement>
    {

        private const string EmailClaimType = "email";
        private const string UserIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache;

        public EmailDomainClaimHandler(
            HttpClient httpClient,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmailDomainRequirement requirement)
        {
            string? emailClaimValue = null;
            if (!HasEmailClaim(context))
            {
                // TID access token did not contain the email claim, check cache for it
                emailClaimValue = CheckCacheForEmail(context);

                // Cache miss, call OIDC userinfo endpoint to try to get it
                if (emailClaimValue == null)
                {
                    emailClaimValue = await GetUserInfoEmailClaimValueAsync(context);
                }
            }
            else
            {
                // TID access token already has the email claim in it, just use that
                emailClaimValue = context.User.Claims.First(c => c.Type.Equals(EmailClaimType, StringComparison.OrdinalIgnoreCase)).Value;
            }

            // We successfully retrieved the email claim for the TID user
            if (!string.IsNullOrEmpty(emailClaimValue))
            {
                // Check if the email address for the TID user belongs to any of the required email domains
                if (requirement.EmailDomains.Any(d => emailClaimValue.EndsWith(d, StringComparison.OrdinalIgnoreCase)))
                {
                    // User email belongs to approved domains list. If this line does not execute, the authorization requirement fails.
                    context.Succeed(requirement);
                }
            }
        }

        /// <summary>
        /// To limit calls to the OIDC userinfo endpoint, there is a 1 hour cache of userinfo lookups by TID uuid
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string? CheckCacheForEmail(AuthorizationHandlerContext context)
        {
            string? email = null;
            var userId = context.User.Claims.FirstOrDefault(c => c.Type.Equals(UserIdClaimType, StringComparison.OrdinalIgnoreCase))?.Value;
            if (userId != null)
            {
                _memoryCache.TryGetValue(userId, out email);
            }

            return email;
        }

        /// <summary>
        /// Uses the OIDC userinfo endpoint to get the email claim for the TID token being checked
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<string?> GetUserInfoEmailClaimValueAsync(AuthorizationHandlerContext context)
        {
            string? userEmailClaimValue = null;

            // Get the TID access token from the incoming request, to call the userinfo endpoint
            string? tidAuthHeader = _httpContextAccessor.HttpContext?.Request?.Headers?.FirstOrDefault(c => c.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase)).Value;

            // Get the userinfo URL
            string? userInfoUrl = _configuration["TrimbleIdentity4:UserInfo"];

            if (!string.IsNullOrEmpty(tidAuthHeader) && !string.IsNullOrEmpty(userInfoUrl))
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(userInfoUrl),
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tidAuthHeader[7..]);

                HttpResponseMessage response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    // Get the email address from the userinfo data returned from the API call
                    JsonDocument jsonResponse = JsonDocument.Parse(await response.Content.ReadAsStreamAsync());
                    if (jsonResponse.RootElement.TryGetProperty(EmailClaimType, out JsonElement jwtJson))
                    {
                        userEmailClaimValue = jwtJson.GetString();
                    }
                }
            }

            // Cache the email address, to reduce call count to userinfo endpoint
            var userId = context.User.Claims.FirstOrDefault(c => c.Type.Equals(UserIdClaimType, StringComparison.OrdinalIgnoreCase))?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                _memoryCache.Set(userId, userEmailClaimValue, DateTimeOffset.UtcNow.AddHours(1));
            }

            return userEmailClaimValue;
        }

        /// <summary>
        /// Queries the user claims on the <paramref name="context"/> object for the email claim type.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool HasEmailClaim(AuthorizationHandlerContext context)
        {
            return context.User.HasClaim(c => c.Type.Equals(EmailClaimType, StringComparison.OrdinalIgnoreCase));
        }
    }
}
