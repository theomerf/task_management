using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Services.Contracts
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterUserAsync(AccountDtoForRegistration accountDto);
        Task<bool> LoginUserAsync(AccountDtoForLogin accountDto);
        Task<LoginResponseDto> GetCurrentUserInfoAsync(string userId);
        Task<TokenDto> CreateTokenAsync(bool populateExp, bool rememberMe);
        Task<TokenDto> RefreshTokenAsync(string refreshToken);
        void SetTokensInsideCookie(TokenDto tokenDto, HttpContext context);
        Task InvalidateRefreshTokenAsync(string userId);
    }
}
