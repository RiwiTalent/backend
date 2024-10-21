using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Services.Groups;
using RiwiTalent.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace RiwiTalent.App.Controllers.Groups
{
    public class GroupCreateController : Controller
    {
        private readonly IValidator<GroupDto> _groupValidator;
        private readonly IGroupCoderRepository _groupRepository;
        private readonly Cloudinary _cloudinary;
        public GroupCreateController(IGroupCoderRepository groupRepository, IValidator<GroupDto> groupValidator, Cloudinary cloudinary)
        {
            _groupRepository = groupRepository;
            _groupValidator = groupValidator;
            _cloudinary = cloudinary;
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

        //upload photo group
        [HttpPost("groups/photo/{groupId}")]
        public async Task<IActionResult> UploadGroupPhoto(string groupId, IFormFile file)
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
                        Transformation = new Transformation().Width(250)
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
                await _groupRepository.UpdateGroupPhoto(groupId, urlPhoto);

                return Ok(new { urlPhoto });
            }
            catch (KeyNotFoundException ex)
            {
                #pragma warning disable
                var problemDetails = StatusError.CreateNotFound(ex.Message, Guid.NewGuid().ToString());
                return StatusCode(problemDetails.Status.Value, problemDetails);
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