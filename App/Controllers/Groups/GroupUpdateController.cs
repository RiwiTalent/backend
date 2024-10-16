using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using RiwiTalent.Domain.Services.Groups;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Shared.Exceptions;

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
        [HttpPut("groups")]
        public async Task<IActionResult> UpdateGroups(GroupCoderDto groupCoderDto)
        {
            if(groupCoderDto is null)
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
                #pragma warning disable
                return StatusCode(problemDetails.Status.Value, problemDetails);
                #pragma warning restore
                throw;
            }

        }

        //endpoint regenerate token
        [HttpPatch]
        [Route("groups/regenerate-token")]
        public async Task<IActionResult> GenerateToken([FromQuery] NewKeyDto newKeyDto)
        {
            try
            {
                await _groupRepository.RegenerateToken(newKeyDto);
                return Ok("The token is already");
                
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