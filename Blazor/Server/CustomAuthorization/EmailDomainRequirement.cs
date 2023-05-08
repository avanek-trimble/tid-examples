using Microsoft.AspNetCore.Authorization;

namespace Blazor.Server.CustomAuthorization
{
    /// <summary>
    /// Custom authorization requirement to authorize against a set of allow-listed email domains
    /// </summary>
    public class EmailDomainRequirement : IAuthorizationRequirement
    {
        public EmailDomainRequirement(params string[] EmailDomains)
        {
            this.EmailDomains = EmailDomains;
        }

        public IEnumerable<string> EmailDomains { get; }
    }
}
