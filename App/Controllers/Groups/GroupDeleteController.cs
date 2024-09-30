using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;

namespace backend.App.Controllers.Groups
{
    public class GroupDeleteController : Controller
    {
        private readonly IGroupCoderRepository _groupCoderRepository;

        public GroupDeleteController(IGroupCoderRepository groupCoderRepository)
        {
            _groupCoderRepository = groupCoderRepository;
        }

        [HttpDelete]
        [Route("group/{Id}")]
        public IActionResult DeleteGroup(string groupId)
        {
            try
            {
                _groupCoderRepository.DeleteGroup(groupId);
                return Ok(new { Message = "The coder has been deleted successfuly" });
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