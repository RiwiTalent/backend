using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Services.Groups;
using RiwiTalent.Infrastructure.ExternalServices;
using RiwiTalent.Infrastructure.Persistence.Repository;
using RiwiTalent.Shared.Exceptions;
using RiwiTalent.Domain.Services.Interface.Login;

namespace RiwiTalent.App.Controllers.Login
{
    public class LoginController : ControllerBase
    {
        private readonly IValidator<UserDto> _validatorUser;
        private readonly MongoDbContext _context;
        private readonly ILoginRepository _loginRepository;

        public LoginController(IValidator<UserDto> validatorUser, MongoDbContext context, ILoginRepository loginRepository)
        {
            _validatorUser = validatorUser;
            _context = context;
            _loginRepository = loginRepository;
        }

        [HttpPost("login/{firebaseToken}")]
        public async Task<IActionResult> Login(string firebaseToken)
        {
            try
            {
                var res = await _loginRepository.GenerateJwtCentinela(firebaseToken);
                
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