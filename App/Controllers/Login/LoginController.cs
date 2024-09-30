using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RiwiTalent.Infrastructure.Data;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;

namespace RiwiTalent.App.Controllers.Login
{
    public class LoginController : ControllerBase
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IValidator<UserDto> _validatorUser;
        private readonly MongoDbContext _context;
        public LoginController(MongoDbContext context, ITokenRepository tokenServices, IValidator<UserDto> validatorUser)
        {
            _context = context;
            _tokenRepository = tokenServices;
            _validatorUser = validatorUser;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] TokenResponseDto tokenResponseDto)
        {
            try
            {
                var users = await _context.Users.Find(u => u.Email == tokenResponseDto.Email && u.Password == tokenResponseDto.Password).FirstOrDefaultAsync();

                //we create a new instance to can validate
                UserDto userDto = new UserDto
                {
                    Email = users.Email,
                    Password = users.Password
                }; 

                var UserValidations = _validatorUser.Validate(userDto);

                if(!UserValidations.IsValid)
                {
                    return Unauthorized(UserValidations.Errors);
                }
                else if(users == null || users.Password != users.Password)
                {
                    var instance = HttpContext.Request.Path + HttpContext.Request.QueryString;
                    return NotFound(StatusError.CreateNotFound("user or password wrong", instance));
                }


                var token = _tokenRepository.GetToken(users);

                return Ok(new { Token = token });
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