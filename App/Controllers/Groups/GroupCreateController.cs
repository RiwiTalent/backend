using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Services.Interface;
using RiwiTalent.Models;
using FluentValidation;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Utils.Exceptions;

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
        [Route("riwitalent/creategroups")]
        // public IActionResult Post([FromBody] GruopCoder groupCoder, CoderDto coderDto)
        public IActionResult Post([FromBody] GroupDto groupDto)
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
                _groupRepository.Add(groupDto);

                return Ok("The Group has been created successfully");
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