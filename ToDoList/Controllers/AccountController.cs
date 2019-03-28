﻿using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClientModels.Errors;
using ClientModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Users;
using ToDoList.Errors;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    [Route("v1/acc")]
    public class AccountController : Controller
    {
        private readonly UserRepository _userRepository;

        public AccountController(UserRepository repository)
        {
            _userRepository = repository;
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> 
            Register([FromBody]UserRegistrationInfo registrationInfo, CancellationToken cancellationToken)
        {
            if (registrationInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("TodoRegistrationInfo");
                return BadRequest(error);
            }

            // TODO Эта операция делается конвертором
            var info = new UserCreationInfo(registrationInfo.Login,
                HashPassword(registrationInfo.Password));
            try
            {
                await _userRepository.CreateAsync(info, cancellationToken);
            }
            catch (UserDuplicationException exception)
            {
                return BadRequest(exception);
            }

            return Ok();
        }

        // Вынести в отдельный класс
        private string HashPassword(string password)
        {
            using (var md5 = MD5.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = md5.ComputeHash(passwordBytes);
                var hash = BitConverter.ToString(hashBytes);
                return hash;
            }
        }
    }
}