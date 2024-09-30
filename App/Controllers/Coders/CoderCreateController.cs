using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;

namespace RiwiTalent.App.Controllers.Coders
{
    public class CoderCreateController : Controller
    {
        private readonly ICoderRepository _coderRepository;
        private readonly IValidator<CoderDto> _coderValidator;
        public CoderCreateController(ICoderRepository coderRepository, IValidator<CoderDto> coderValidator)
        {
            _coderRepository = coderRepository;
            _coderValidator = coderValidator;
        }

        //Endpoint
        [HttpPost]
        [Route("coders")]
        public IActionResult Post([FromBody] CoderDto coderDto)
        {


            if(!ModelState.IsValid)
            {
                var instance = Guid.NewGuid().ToString();
                var problemDetails = StatusError.CreateBadRequest(instance);
                return BadRequest(problemDetails);
            }


            //validations with FluentValidation
            var ValidationResult = _coderValidator.Validate(coderDto);

            if(!ValidationResult.IsValid)
            {
                return BadRequest(ValidationResult.Errors);
            }

            try
            {
                _coderRepository.Add(coderDto);
                return Ok("The coder has been created successfully");
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