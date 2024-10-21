using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Domain.Services.Interface.Coders;
using RiwiTalent.Shared.Exceptions;

namespace RiwiTalent.App.Controllers.Groups
{
    public class GroupDeleteController : Controller
    {
        private readonly ICoderStatusHistoryRepository _coderStatusHistoryRepository;
        public GroupDeleteController(ICoderStatusHistoryRepository coderStatusHistoryRepository)
        {
            _coderStatusHistoryRepository = coderStatusHistoryRepository;
        }

        //endpoint
        [HttpDelete]
        [Route("coders/{coderId}/group/{groupId}")]
        public async Task<IActionResult> DeleteCoder(string coderId, string groupId)
        {
            try
            {
                await _coderStatusHistoryRepository.DeleteCoderGroup(coderId, groupId);
                return Ok("The coder has been deleted successfuly");
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