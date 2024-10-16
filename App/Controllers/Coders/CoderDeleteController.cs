using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;

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
        [Route("coder/{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            /* The function has the main principle of search by coder id
                and then update status the Active to Inactive
            */
            try
            {                
                _coderRepository.Delete(id);               
                return Ok(new { Message = "The status of coder has been updated to Inactive" });
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


    