using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Services.Interface;

namespace RiwiTalent.App.Controllers.Groups
{
    public class DeleteCoderGroupController : Controller
    {
        private readonly IGroupCoderRepository _groupRepository;
        public DeleteCoderGroupController(IGroupCoderRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        //endpoint
        [HttpDelete]
        [Route("riwitalent/{id:length(24)}/deletecodergroup")]
        public async Task<IActionResult> DeleteCoder(string id)
        {
            try
            {
                await _groupRepository.DeleteCoderGroup(id);
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