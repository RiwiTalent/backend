using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Services.Interface;

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
        [Route("riwitalent/{id:length(24)}/deleteCoderGroup")]
        public async Task<IActionResult> DeleteCoder(string id)
        {
            try
            {
                await _coderStatusHistoryRepository.DeleteCoderGroup(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                throw;
            }
            
        }
    }
}