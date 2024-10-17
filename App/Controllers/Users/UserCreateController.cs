using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Domain.Entities;
using RiwiTalent.Domain.Services.Interface.Users;
using RiwiTalent.Shared.Exceptions;

namespace RiwiTalent.App.Controllers.Users
{
    public class UserCreateController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserCreateController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //endpoint
        [HttpPost("users")]
        public async Task<IActionResult> Post(User user)
        {

            if(!ModelState.IsValid)
            {
                var instance = Guid.NewGuid().ToString();
                var problemDetails = StatusError.CreateBadRequest(instance);
                return BadRequest(problemDetails);
            }

            try
            {
                await _userRepository.Add(user);
                return Ok("The user has been created successfully");
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