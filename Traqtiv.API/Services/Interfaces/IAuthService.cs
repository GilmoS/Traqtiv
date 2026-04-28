using Traqtiv.API.Models.DTOs.Requests;
using Traqtiv.API.Models.DTOs.Responses;

namespace Traqtiv.API.Services.Interfaces
{
    /// Defines authentication operations for registering new users and signing in existing users.
    public interface IAuthService
    {
        
        /// Registers a new user with the specified registration information.
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

        /// Authenticates a user using the provided login credential
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    }
}
