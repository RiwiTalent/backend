using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;
using RiwiTalent.Utils.ExternalKey;

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
        [Route("group/{name}")]
        public async Task<IActionResult> GetCodersByGroup(string name)
        {
            try
            {
                var groupExist = await _groupRepository.GroupExistByName(name);
                if (!groupExist)
                {
                    var instance = HttpContext.Request.Path + HttpContext.Request.QueryString;
                    return NotFound(StatusError.CreateNotFound($"The group '{name}' not exists.", instance));
                }

                var coder = await _coderRepository.GetCodersByGroup(name);
                if (coder == null || !coder.Any())
                {  
                    var instance = HttpContext.Request.Path + HttpContext.Request.QueryString;
                    return NotFound(StatusError.CreateNotFound($"This group haven't coders yet, '{name}'.", instance));
                }
                return Ok(coder);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error to get the coders of grupo '{name}'", ex);
            }
        }

        //obtener el uuid y revertirlo
        [HttpPost]
        [Route("validation-external")]
        public async Task<IActionResult> GetUUID([FromBody] KeyDto keyDto)
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
        [Route("group-details/{id}")]
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
