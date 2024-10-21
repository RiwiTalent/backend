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
                #pragma warning disable
                return Ok(new { Token = res.Access_token });
                #pragma warning restore
            }
            catch (KeyNotFoundException ex)
            {
                var problemDetails = StatusError.CreateNotFound($"No token found by {firebaseToken} or not exist.", Guid.NewGuid().ToString());
                return StatusCode(problemDetails.Status.Value, problemDetails);
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                #pragma warning disable
                return StatusCode(problemDetails.Status.Value, problemDetails);
                #pragma warning restore
                throw;
            }
        }
    }
}