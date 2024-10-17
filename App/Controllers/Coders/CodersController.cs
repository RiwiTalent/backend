using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Domain.Services.Interface.Coders;
using RiwiTalent.Shared.Exceptions;

namespace RiwiTalent.App.Controllers
{
    public class CodersController : Controller
    {
        private readonly ICoderRepository _coderRepository;
        public CodersController(ICoderRepository coderRepository)
        {
            _coderRepository = coderRepository;
        }

        //get all coders
        /* [Authorize] */
        [HttpGet]
        [Route("coders")]
        public async Task<IActionResult> GetCoders([FromQuery] List<string>? skills)
        {
            if (!ModelState.IsValid)
            {
                var instance = Guid.NewGuid().ToString();
                var problemDetails = StatusError.CreateBadRequest(instance);
                return BadRequest(problemDetails);
            }

            try
            {
                // Obtener todos los coders del repositorio
                var coders = await _coderRepository.GetCoders(skills);
                return Ok(coders);
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


        //Get all coders pagination
        [HttpGet]
        [Route("coders/page={page}")]
        public async Task<IActionResult> Get(int page = 1,int cantRegisters = 10)
        {
            /*The main idea of this method, is when the user list all coders can watch for pagination*/
            try
            {
                var coderPagination = await _coderRepository.GetCodersPagination(page, cantRegisters);

                if(coderPagination == null)
                {
                    var instance = Guid.NewGuid().ToString();
                    var problemDetails = StatusError.CreateBadRequest(instance);
                    return BadRequest(problemDetails);
                }
                return Ok(new 
                {
                    Page = page,
                    Registers = coderPagination.Count(),
                    Coders = coderPagination,
                    TotalPages = coderPagination.TotalPages,
                    PageBefore = coderPagination.PageBefore,
                    PageAfter = coderPagination.PageAfter
                });
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

        //Get coder by id
        [HttpGet]
        [Route("coder/{id}")]
        public async Task<IActionResult> GetCoderById(string id)
        {
            
            try
            {
                var coder = await _coderRepository.GetCoderId(id);

                if(coder == null)
                {
                    var instance = Guid.NewGuid().ToString();
                    var problemDetails = StatusError.CreateBadRequest(instance);
                    return BadRequest(problemDetails);
                }

                return Ok(coder);
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

        //Get Coder by name
        [HttpGet]
        [Route("coders/name")]
        public async Task<IActionResult> GetCoderByName(string name)
        {
            try
            {
                var coder = await _coderRepository.GetCoderName(name);

                if (coder is null)
                {
                    var instance = HttpContext.Request.Path + HttpContext.Request.QueryString;
                    return NotFound(StatusError.CreateNotFound($"The coder {name} not found.", instance));
                }

                return Ok(coder);
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


        //Get coder by skill tecnical
        [HttpGet]
        [Route("coders/skill")]
        public async Task<IActionResult> GetCodersBySkill([FromQuery] List<string> skill)
        {
            try
            {
                var coders = await _coderRepository.GetCodersBySkill(skill);
                if (coders is null || !coders.Any())
                {
                    var instance = HttpContext.Request.Path + HttpContext.Request.QueryString;
                    return NotFound(StatusError.CreateNotFound("There isn't coder with those languages.", instance));
                }

                return Ok(coders);
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

        //Get coder by language level in english
        [HttpGet]
        [Route("coders/language-level={level}")]
        public async Task<IActionResult> GetCodersByLanguage([FromRoute] string level, string language = "English")
        {
            try
            {
                var coders = await _coderRepository.GetCodersBylanguage(level);
                if (coders is null || !coders.Any())
                {
                    var instance = HttpContext.Request.Path + HttpContext.Request.QueryString;
                    return NotFound(StatusError.CreateNotFound("There isn't coder with those language level.", instance));
                }
                return Ok(coders);
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

        // [HttpPost("filterBySkills")]
        // public async Task<IActionResult> FilterCodersBySkills([FromBody] List<string> selectedSkills)
        // {
        //     try
        //     {
        //         // Llamada asíncrona al repositorio
        //         var filteredCoders = await _coderRepository.FilterBySkills(selectedSkills);

        //         // Devuelve los coders filtrados en el cuerpo de la respuesta
        //         return Ok(filteredCoders);
        //     }
        //     catch (Exception ex)
        //     {
        //         // Manejo de errores mediante el objeto ProblemDetails
        //         var problemDetails = StatusError.CreateInternalServerError(ex);
        //         return StatusCode(problemDetails.Status.Value, problemDetails);
        //     }
        // }
    }
}
