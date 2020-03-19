using Microsoft.AspNetCore.Authorization;

namespace Angus.Bills.Auth
{
    public class AuthAttribute : AuthorizeAttribute
    {
        public AuthAttribute(string scheme, string policy = "") : base(policy)
        {
            AuthenticationSchemes = scheme;
        }
    }
}