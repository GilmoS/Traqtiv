using Traqtiv.API.DAL.Interfaces;
using Traqtiv.API.Helpers;
using Traqtiv.API.Models.DTOs.Requests;
using Traqtiv.API.Models.DTOs.Responses;
using Traqtiv.API.Models.Entities;
using Traqtiv.API.Services.Interfaces;

namespace Traqtiv.API.Services
{
    /// Provides authentication operations such as user registration and login.
    public class AuthService : IAuthService
    {
        private readonly ISmartFitnessDal _dal;
        private readonly IConfiguration _configuration;

        /// Initializes a new instance of the AuthService" class.
        public AuthService(ISmartFitnessDal dal, IConfiguration configuration)
        {
            _dal = dal;
            _configuration = configuration;
        }

        /// Registers a new user with the provided registration details.
        ///  This method performs basic validation (email, password length, name) using ValidationHelper.
        /// If validation passes and the email is not already registered, the password is hashed using PasswordHelper
        /// and the user is persisted via ISmartFitnessDal. A JWT is generated with JwtHelper.GenerateToken.
        
        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            if (!ValidationHelper.IsValidEmail(request.Email))
                return new AuthResponseDto { Success = false, Message = "Invalid email address" };

            if (!ValidationHelper.IsValidPassword(request.Password))
                return new AuthResponseDto { Success = false, Message = "Password must be at least 6 characters" };

            if (!ValidationHelper.IsValidName(request.FirstName) || !ValidationHelper.IsValidName(request.LastName))
                return new AuthResponseDto { Success = false, Message = "Invalid name" };

            var existingUser = await _dal.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
                return new AuthResponseDto { Success = false, Message = "Email already exists" };

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = PasswordHelper.HashPassword(request.Password),
                DateOfBirth = request.DateOfBirth.ToUniversalTime()
            };

            await _dal.AddUserAsync(user);

            var token = JwtHelper.GenerateToken(user.Id, user.Email, _configuration);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Registration successful",
                Token = token,
                UserId = user.Id
            };
        }

        /// Authenticates a user with the provided credentials.
        /// This method performs a syntactic email check, retrieves the user by email from ISmartFitnessDal
        /// verifies the provided password against the stored hash using PasswordHelper.VerifyPassword
        /// and generates a JWT withJwtHelper.GenerateToken on success.
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            
            if (!ValidationHelper.IsValidEmail(request.Email))
                return new AuthResponseDto { Success = false, Message = "Invalid email address" };

            
            var user = await _dal.GetUserByEmailAsync(request.Email);
            if (user == null)
                return new AuthResponseDto { Success = false, Message = "Invalid email or password" };

           
            if (!PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
                return new AuthResponseDto { Success = false, Message = "Invalid email or password" };

            var token = JwtHelper.GenerateToken(user.Id, user.Email, _configuration);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Login successful",
                Token = token,
                UserId = user.Id
            };
        }
    }
}
