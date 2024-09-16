using backend.Services.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RiwiTalent.Infrastructure.Data;
using RiwiTalent.Models.DTOs;

namespace backend.App.Controllers.Login
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

        [HttpPost("riwitalent/login")]
        public async Task<IActionResult> Login([FromBody] TokenResponseDto tokenResponseDto)
        {
            var users = await _context.Users.Find(u => u.Email == tokenResponseDto.Email && u.Password == tokenResponseDto.Password).FirstOrDefaultAsync();

            UserDto userDto = new UserDto
            {
                Email = users.Email,
                Password = users.Password
            }; //we create a new instance to can validate

            var UserValidations = _validatorUser.Validate(userDto);

            if(!UserValidations.IsValid)
            {
                return Unauthorized(UserValidations.Errors);
            }
            else if(users == null || users.Password != users.Password)
            {
                return NotFound("user or password wrong");
            }


            var token = _tokenRepository.GetToken(users);

            return Ok(new { Token = token });
        }
    }
}