using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Domain.Services.Groups;
using RiwiTalent.Shared.Exceptions;

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
        [Route("groups/{groupId}")]
        public IActionResult DeleteGroup([FromRoute] string groupId)
        {
            try
            {
                _groupCoderRepository.DeleteGroup(groupId);
                return Ok(new { Message = "The group has been deleted successfuly" });
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