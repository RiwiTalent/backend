using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Entities;
using RiwiTalent.Domain.Services.Interface.Coders;
using RiwiTalent.Shared.Exceptions;

namespace RiwiTalent.App.Controllers.Coders
{
    public class CoderCreateController : Controller
    {
        private readonly ICoderRepository _coderRepository;
        private readonly IValidator<CoderDto> _coderValidator;
        private readonly Cloudinary _cloudinary;
        public CoderCreateController(ICoderRepository coderRepository, IValidator<CoderDto> coderValidator, Cloudinary cloudinary)
        {
            _coderRepository = coderRepository;
            _coderValidator = coderValidator;
            _cloudinary = cloudinary;
        }

        //Endpoint
        [HttpPost]
        [Route("coders")]
        public async Task<IActionResult> Post([FromBody] CoderDto coderDto)
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
                await _coderRepository.Add(coderDto);
                return Ok("The coder has been created successfully");
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }
        }

        //upload photo
        [HttpPost("upload-photo/{coderId}")]
        public async Task<IActionResult> UploadCoderPhoto(string coderId, IFormFile file)
        {   
            if(file == null || file.Length == 0)
            {
                var instance = Guid.NewGuid().ToString();
                var problemDetails = StatusError.CreateBadRequest(instance);
                return BadRequest($"{problemDetails}, No file uploaded");

            }

            try
            {
                var uploadResult = new ImageUploadResult();

                using(var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Gravity("face")
                                                            .Width(250)
                                                            .Height(300)
                                                            .Crop("scale")
                                                            .Chain()
                                                            .Quality("auto")
                                                            .Chain()
                                                            .FetchFormat("auto")
                    };

                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }

                if(uploadResult.Error != null)
                    return BadRequest(uploadResult.Error.Message);
                
                var urlPhoto = uploadResult.SecureUrl.AbsoluteUri;
                await _coderRepository.UpdateCoderPhoto(coderId, urlPhoto);

                return Ok(new { urlPhoto });
            }
            catch (Exception ex)
            {
                var problemDetails = StatusError.CreateInternalServerError(ex);
                return StatusCode(problemDetails.Status.Value, problemDetails);
                throw;
            }

        }

        //upload Cv
        [HttpPost("upload-pdf/{coderId}")]
        public async Task<IActionResult> UploadPdfCv(string coderId, IFormFile file)
        {
            if(file == null || file.Length == 0)
            {
                var instance = Guid.NewGuid().ToString();
                var problemDetails = StatusError.CreateBadRequest(instance);
                return BadRequest($"{problemDetails}, No file uploaded");

            }

             try
            {
                var uploadResult = new RawUploadResult();

                using(var stream = file.OpenReadStream())
                {
                    var uploadParams = new RawUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        PublicId = $"cv_{coderId}_document" + ".pdf"
                    };

                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }

                if(uploadResult.Error != null)
                    return BadRequest(uploadResult.Error.Message);

                var urlCv = uploadResult.SecureUrl.AbsoluteUri;
                await _coderRepository.UpdateCoderCv(coderId, urlCv);

                return Ok(new { urlCv });
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