using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Services.Interface;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Models;
using AutoMapper;
using RiwiTalent.Utils.Exceptions;

namespace RiwiTalent.App.Controllers.Groups
{

    public class GroupUpdateController : Controller
    {
        private readonly IGroupCoderRepository _groupRepository;
        private readonly IMapper _mapper;
        public GroupUpdateController(IGroupCoderRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        //Endpoint
        [HttpPut]
        [Route("riwitalent/updategroups")]
        public async Task<IActionResult> UpdateGroups(GroupCoderDto groupCoderDto)
        {
            if(!ModelState.IsValid)
            {
                var instance = Guid.NewGuid().ToString();
                var problemDetails = StatusError.CreateBadRequest(instance);
                return BadRequest(problemDetails);
            }
            
            try
            {
                await _groupRepository.Update(groupCoderDto);
                return Ok("The Group has been updated the correct way");
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