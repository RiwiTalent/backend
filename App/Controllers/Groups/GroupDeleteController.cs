using Microsoft.AspNetCore.Mvc;
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
                return Ok(new { Message = "The group has been deleted successfuly" });
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