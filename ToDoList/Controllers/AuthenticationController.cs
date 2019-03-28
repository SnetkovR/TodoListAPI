using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using ClientModels.Errors;
using ClientModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.Users;
using ToDoList.Errors;
using ToDoList.JWT;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly UserRepository _userRepository;

        public AuthenticationController(UserRepository repository)
        {
            _userRepository = repository;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<string> Post([FromBody]UserRegistrationInfo authRequest,
            [FromServices] IJwtSigningEncodingKey signingEncodingKey, CancellationToken cancellationToken)
        {
            if (authRequest == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing(nameof(authRequest));
                return BadRequest(error);
            }

            var user = _userRepository.GetAsync(authRequest.Login, cancellationToken);
            if (user.Result == null)
            {
                var error = ServiceErrorResponses.UserNotFound(authRequest.Login);
                return BadRequest(error);
            }

            if (user.Result.PasswordHash != Authenticator.HashPassword(authRequest.Password))
            {
                var error = ServiceErrorResponses.IncorrectPassword();
                return BadRequest(error);
            }

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, authRequest.Login),
            };

            var token = new JwtSecurityToken(
                issuer: "TodoListApp",
                audience: "Client",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: new SigningCredentials(
                    signingEncodingKey.GetKey(),
                    signingEncodingKey.SigningAlgorithm)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }

    }
}
