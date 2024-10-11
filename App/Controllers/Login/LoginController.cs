using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RiwiTalent.Infrastructure.Data;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;
using System.Security.Cryptography;

namespace RiwiTalent.App.Controllers.Login
{
    public class LoginController : ControllerBase
    {
        private readonly IValidator<UserDto> _validatorUser;
        private readonly MongoDbContext _context;
        private readonly ILogginRepository _logginRepository;

        public LoginController(IValidator<UserDto> validatorUser, MongoDbContext context, ILogginRepository logginRepository)
        {
            _validatorUser = validatorUser;
            _context = context;
            _logginRepository = logginRepository;
        }

        [HttpPost("login/{firebaseToken}")]
        public async Task<IActionResult> Login(string firebaseToken)
        {
            try
            {
                var res = await _logginRepository.GenerateJwtCentinela(firebaseToken);
                
                return Ok(new { Token = res.access_token });
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }
        }
    }
}