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

        [HttpPatch]
        [Route("group/{id:length(24)}/reactivate")]
        public async Task<IActionResult> ReactivateGroup(string id)
        {
            /* The function has the main principle of search by group id
                and then update status the Inactive to Active
            */
            try
            {
                await _groupRepository.ReactiveGroup(id);
                return Ok(new { Message = "The status of group has been updated to Active" });
            }
            catch (KeyNotFoundException ex)
            {
                var problemDetails = StatusError.CreateNotFound(ex.Message, Guid.NewGuid().ToString());
                return StatusCode(problemDetails.Status.Value, problemDetails);
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
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