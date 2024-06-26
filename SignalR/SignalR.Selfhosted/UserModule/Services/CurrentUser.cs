using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SignalR.Api.UserModule.Services
{
    /// <summary>
    /// Class for current user object.
    /// </summary>
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc/>
        public int Id => int.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out int id) ? id : 0;
    }
}
