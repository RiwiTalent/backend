using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Entities;
using RiwiTalent.Domain.Services.Interface.Users;
using RiwiTalent.Shared.Exceptions;

namespace RiwiTalent.App.Controllers.Users
{
    public class UserUpdateController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserUpdateController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //endpoint
        [HttpPut("users")]
        public async Task<IActionResult> UpdateUser(UserDto userDto)
        {
            if(userDto == null)
            {
                var instance = Guid.NewGuid().ToString();
                var problemDetails = StatusError.CreateBadRequest(instance);
                return BadRequest(problemDetails);
            }

            try
            {
                await _userRepository.Update(userDto);
                return Ok("The user has been updated the correct way");
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