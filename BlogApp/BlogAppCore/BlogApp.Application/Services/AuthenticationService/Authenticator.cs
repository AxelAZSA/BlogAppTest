

using BlogApp.Application.Services.TokenService;
using BlogApp.Core.Domain.IRepository;
using BlogApp.Core.Domain.Models;
using BlogApp.Core.Domain.Models.Response;
using BlogApp.Core.Models.Tokens;

namespace BlogApp.Application.Services.AuthenticationService
{
    public class Authenticator
    {
        private readonly TokenGenerator _tokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public Authenticator(TokenGenerator tokenGenerator, RefreshTokenGenerator refreshTokenGenerator, IRefreshTokenRepository refreshTokenRepository)
        {
            _tokenGenerator = tokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthenticationResponse> Authentication(User existingUser)
        {
            if (existingUser == null)
            {
                throw new ArgumentNullException(nameof(existingUser), "El usuario no puede ser nulo.");
            }

            string token;
            string refreshToken;

            try
            {
                token = _tokenGenerator.GenerateToken(existingUser);
                refreshToken = _refreshTokenGenerator.GenerateToken();

                RefreshToken refreshClienteToken = new RefreshToken()
                {
                    token = refreshToken,
                    idUser = existingUser.Id,
                    role = "User"
                };

                await _refreshTokenRepository.CreateRefreshToken(refreshClienteToken);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                throw new Exception("Error al autenticar al usuario", ex);
            }

            return new AuthenticationResponse()
            {
                token = token,
                refreshToken = refreshToken
            };
        }

    }
}
