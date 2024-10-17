using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Domain.Services.Interface.Coders;
using RiwiTalent.Shared.Exceptions;

namespace RiwiTalent.App.Controllers.Coders
{
    public class CoderDeleteController : Controller
    {
        private readonly ICoderRepository _coderRepository;
        public CoderDeleteController(ICoderRepository coderRepository)
        {
            _coderRepository = coderRepository;
        }
    
        [HttpDelete]
        [Route("coders/{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            /* The function has the main principle of search by coder id
                and then update status the Active to Inactive
            */
            try
            {                
                await _coderRepository.Delete(id);               
                return Ok(new { Message = "The status of coder has been updated to Inactive" });
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

        [HttpDelete]
        [Route("coder-groups/{id:length(24)}")]
        public IActionResult DeleteCoderOfGroup(string id)
        {
            /* The function has the main principle of search by coder id
                and then update status the Active to Inactive
            */
            try
            {                
                _coderRepository.DeleteCoderGroup(id);               
                return Ok(new { Message = "The status of coder has been updated to Active" });
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


    