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
                and then update status the Activo to Inactivo
            */
            try
            {                
                await _coderRepository.Delete(id);               
                return Ok(new { Message = "The status of coder has been updated to Inactivo" });
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
        public IActionResult DeleteCoderOfGroup(string coderId, string groupId)
        {
            /* The function has the main principle of search by coder id
                and then update status the Activo to Inactivo
            */
            try
            {                
                _coderRepository.DeleteCoderOfGroup(coderId, groupId);               
                return Ok(new { Message = "The status of coder has been updated to Activo" });
            }
            catch (KeyNotFoundException ex)
            {
                var problemDetails = StatusError.CreateNotFound(ex.Message, Guid.NewGuid().ToString());
                return StatusCode(problemDetails.Status.Value, problemDetails);
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


    