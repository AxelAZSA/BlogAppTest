using BlogApp.Application.Services;
using BlogApp.Application.Services.AuthenticationService;
using BlogApp.Application.Services.TokenService;
using BlogApp.Core.Domain.DTOs;
using BlogApp.Core.Domain.Interfaces;
using BlogApp.Core.Domain.IRepository;
using BlogApp.Core.Domain.Models;
using BlogApp.Core.Domain.Models.Response;
using BlogApp.Core.Domain.Models.Tokens;
using BlogApp.Core.Models.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApp.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _UserRepository;
        private readonly BcryptPassword _passwordHasher;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly Authenticator _authenticator;

        public UsersController(IUserRepository UserRepository, BcryptPassword passwordHasher, IRefreshTokenRepository refreshTokenRepository, Authenticator authenticator, RefreshTokenValidator refreshTokenValidator)
        {
            _UserRepository = UserRepository;
            _passwordHasher = passwordHasher;
            _authenticator = authenticator;
            _refreshTokenValidator = refreshTokenValidator;
            _refreshTokenRepository = refreshTokenRepository;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _UserRepository.GetAll());
        }

        [Authorize]
        [HttpGet("MyUser")]
        public async Task<IActionResult> GetUser()
        {

            string rawUserId = HttpContext.User.FindFirstValue("id");
            if (!int.TryParse(rawUserId, out int userId))
            {
                return Unauthorized();
            }
            var user = await _UserRepository.GetById(userId);

            if (user == null)
                return NotFound();

            UserDto userDto = new UserDto()
            {
                id = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email
            };

            return Ok(userDto);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO userTemp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var existingUser = await _UserRepository.GetByCorreo(userTemp.email);

            if (existingUser != null)
            {
                return Conflict(new ErrorResponse("Ya hay una cuenta con ese correo"));
            }

            if (userTemp.password != userTemp.confirmPassword)
                return BadRequest(new ErrorResponse("Las contraseñas no coinciden"));

            LoginDto login = new LoginDto()
            {
                email = userTemp.email,
                password = userTemp.password
            };

            userTemp.password = _passwordHasher.Hash(userTemp.password);

            await _UserRepository.Create(userTemp);

            return await Login(login);
        }


        [Authorize]
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Put(int id, User User)
        {
            if (id != User.Id)
            {
                return BadRequest();
            }

            await _UserRepository.Update(User);

            return Ok("Actualizado con exito");
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var existingUser = await _UserRepository.GetByCorreo(login.email);


            if (existingUser == null)
            {
                return Unauthorized();
            }
            bool verifyPassword = _passwordHasher.Verify(login.password, existingUser.PasswordHash);
            if (!verifyPassword)
            {
                return Unauthorized();
            }

            return Ok(await _authenticator.Authentication(existingUser));
        }

        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            string rawUserId = HttpContext.User.FindFirstValue("id");
            if (!int.TryParse(rawUserId, out int userId))
            {
                return Unauthorized();
            }
            await _refreshTokenRepository.DeleteAll(userId);
            return NoContent();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }
            bool validateToken = _refreshTokenValidator.Validate(request.RefreshToken);
            if (!validateToken)
            {
                return BadRequest(new ErrorResponse("Invalid Refresh Token"));
            }
            RefreshToken refreshTokenDTO = await _refreshTokenRepository.GetByToken(request.RefreshToken);
            if (refreshTokenDTO == null)
            {
                return NotFound(new ErrorResponse("Invalid refresh token"));
            }
            await _refreshTokenRepository.DeleteRefreshToken(refreshTokenDTO.id);
            User user = await _UserRepository.GetById(refreshTokenDTO.idUser);
            if (User == null)
            {
                return NotFound(new ErrorResponse("User doesn´t exist"));
            }

            return Ok(await _authenticator.Authentication(user));
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponse(errors));
        }
    }
}
