using ActionCommandGame.Abstractions;
using ActionCommandGame.Api.Authentication.Extensions;
using Microsoft.AspNetCore.Http;

namespace ActionCommandGame.Api.Authentication
{
    public class HttpContextUserContext: IUserContext<string?>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId
        {
            get => _httpContextAccessor.HttpContext?.User?.GetId();
            set => throw new NotImplementedException();
        }
    }
}
