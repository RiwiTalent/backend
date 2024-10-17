using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Services.Groups;
using RiwiTalent.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace RiwiTalent.App.Controllers.Groups
{
    public class GroupCreateController : Controller
    {
        private readonly IValidator<GroupDto> _groupValidator;
        private readonly IGroupCoderRepository _groupRepository;
        public GroupCreateController(IGroupCoderRepository groupRepository, IValidator<GroupDto> groupValidator)
        {
            _groupRepository = groupRepository;
            _groupValidator = groupValidator;
        }

        //endpoint
        [HttpPost]
        [Route("groups")]
        // public IActionResult Post([FromBody] GruopCoder groupCoder, CoderDto coderDto)
        public async Task<IActionResult> Post([FromBody] GroupDto groupDto)
        {
            //we create a new instance to can validate
            if(groupDto is null)
            {
                return NotFound("GroupCoderDto not found.");
            } 

            //validations with FluentValidation
            var GroupValidations = _groupValidator.Validate(groupDto);

            if(!GroupValidations.IsValid)
            {
                return BadRequest(GroupValidations.Errors);
            }

            try
            {
                await _groupRepository.Add(groupDto);
                return Ok("The Group has been created successfully");
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