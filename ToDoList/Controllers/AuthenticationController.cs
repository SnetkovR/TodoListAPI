using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ClientModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ToDoList.JWT;

namespace ToDoList.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<string> Post(User authRequest,
            [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, authRequest.Login),
            };

            var token = new JwtSecurityToken(
                issuer: "TodoLostApp",
                audience: "Client",
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: new SigningCredentials(
                    signingEncodingKey.GetKey(),
                    signingEncodingKey.SigningAlgorithm)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
