using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Services.Interface;

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
        [Route("riwitalent/createcoders")]
        public IActionResult Post([FromBody] CoderDto coderDto)
        {


            if(coderDto == null)
            {
                return BadRequest(Utils.Exceptions.StatusError.CreateBadRequest());
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
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                throw;
            }
        }

    }
}