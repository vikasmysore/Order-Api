using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserInfoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetEmail()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("emails")?.Value;
        }

        public string GetUsername()
        {
            return GetEmail();
        }

        public string GetGivenName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.GivenName)?.Value;
        }

        public string GetFamilyName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Surname)?.Value;
        }

        public string GetDisplayName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("name")?.Value;
        }
    }
}
