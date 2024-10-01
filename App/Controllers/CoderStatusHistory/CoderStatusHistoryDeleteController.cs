using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;

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
        [Route("coder/group/{id:length(24)}")]
        public async Task<IActionResult> DeleteCoder(string id)
        {
            try
            {
                await _coderStatusHistoryRepository.DeleteCoderGroup(id);
                return Ok("The coder has been deleted successfuly");
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