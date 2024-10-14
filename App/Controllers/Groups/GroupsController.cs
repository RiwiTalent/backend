using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.ExternalKey;
using RiwiTalent.Domain.Services.Groups;
using RiwiTalent.Domain.Services.Interface.Coders;
using RiwiTalent.Shared.Exceptions;

namespace RiwiTalent.App.Controllers.Groups
{

    public class GroupsController : Controller
    {
        private readonly IGroupCoderRepository _groupRepository;
        private readonly ICoderRepository _coderRepository;
        private readonly ExternalKeyUtils _service;
        public GroupsController(IGroupCoderRepository groupRepository, ICoderRepository coderRepository, ExternalKeyUtils service)
        {
            _groupRepository = groupRepository;
            _coderRepository = coderRepository;
            _service = service;
        }

        [HttpGet]
        [Route("groups")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var groupList = await _groupRepository.GetGroupCoders();

                if(groupList is null)
                {
                    var instance = HttpContext.Request.Path + HttpContext.Request.QueryString;
                    return NotFound(StatusError.CreateNotFound("The groups not found", instance));
                }

                return Ok(groupList);
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }
        }

        [HttpGet]
        [Route("groups/inactive")]
        public async Task<IActionResult> GetGroupsInactive()
        {
            try
            {
                var groupList = await _groupRepository.GetGroupsInactive();
                if (groupList == null)
                {
                    return NotFound("There are no inactive groups.");
                }
                return Ok(groupList);
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }
        }

        [HttpGet]
        [Route("groups/active")]
        public async Task<IActionResult> GetGroupsActive()
        {
            try
            {
                var groupList = await _groupRepository.GetGroupsActive();
                if (groupList == null)
                {
                    return NotFound("There are no active groups.");
                }
                return Ok(groupList);
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }
        }

        //Get coders by group
        [HttpGet]
        [Route("groups/{name}")]
        public async Task<IActionResult> GetGroupByName(string name)
        {
            try
            {
                

                var group = await _groupRepository.GetGroupByName(name);
                if (group == null)
                {  
                    var instance = HttpContext.Request.Path + HttpContext.Request.QueryString;
                    return NotFound(StatusError.CreateNotFound($"This group haven't coders yet, '{name}'.", instance));
                }
                return Ok(group);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error to get the coders of grupo '{name}'", ex);
            }
        }

        
        [HttpPost]
        [Route("company/validate-external")]
        public async Task<IActionResult> GetCompanyCredentials([FromBody] KeyDto keyDto)
        {
            try
            {
                var result = await _groupRepository.SendToken(keyDto);
                
                if(result != null)
                    return Ok(new {Message = "you've access", Email = keyDto.AssociateEmail });
                return NotFound("Access denied"); 
            }
            catch(Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }
        }

        [HttpGet]
        [Route("groups/{id}/details")]
        public async Task<IActionResult> GetGroupInfoById(string id)
        {
            try
            {
                GroupDetailsDto groupInfo = await _groupRepository.GetGroupInfoById(id);
                if(groupInfo is null)
                {
                    var instance = HttpContext.Request.Path + HttpContext.Request.QueryString;
                    return NotFound(StatusError.CreateNotFound($"The group {id} not found", instance));
                }

                return Ok(groupInfo);
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
