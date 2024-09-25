using Microsoft.AspNetCore.Mvc;
using RiwiTalent.Services.Interface;

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
        [HttpGet]
        [Route("riwitalent/coders")]
        public async Task<IActionResult> Get()
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(RiwiTalent.Utils.Exceptions.StatusError.CreateBadRequest());
            }

            try
            {
                var coders = await _coderRepository.GetCoders();
                return Ok(coders);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                throw;
            }
        }


        //Get all coders pagination
        [HttpGet]
        [Route("riwitalent/coders/page={page}")]
        public async Task<IActionResult> Get(int page = 1,int cantRegisters = 10)
        {
            /*The main idea of this method, is when the user list all coders can watch for pagination*/
            try
            {
                var coderPagination = await _coderRepository.GetCodersPagination(page, cantRegisters);

                if(coderPagination == null)
                {
                    return BadRequest(RiwiTalent.Utils.Exceptions.StatusError.CreateBadRequest());
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
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                throw;
            }
        }

        //Get coder by id
        [HttpGet]
        [Route("riwitalent/{id}/coder")]
        public async Task<IActionResult> GetCoderById(string id)
        {
            try
            {
                var coder = await _coderRepository.GetCoderId(id);

                if (coder is null)
                {
                    return NotFound(new { message = $"Coder con ID {id} no fue encontrado." });
                }

                return Ok(coder);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                throw;
            }
        }

        //Get Coder by name
        [HttpGet]
        [Route("riwitalent/{name}/coders")]
        public async Task<IActionResult> GetCoderByName(string name)
        {
            try
            {
                var coder = await _coderRepository.GetCoderName(name);

                if (coder is null)
                {
                    return NotFound( new{ message = $"Coder con nombre {name} no fue encontrado."});
                }

                return Ok(coder);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                throw;
            }
        }


        //Get coder by skill tecnical
        [HttpGet]
        [Route("RiwiTalent/skill/coder")]
        public async Task<IActionResult> GetCodersBySkill([FromQuery] List<string> skill)
        {
            try
            {
                var coders = await _coderRepository.GetCodersBySkill(skill);
                if (coders is null || !coders.Any())
                {
                    return NotFound("No hay coder con esos lenguajes.");
                }

                return Ok(coders);
            }
            catch (Exception ex)
            {
                
                 return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }

        //Get coder by language level in english
        [HttpGet]
        [Route("RiwiTalent/coder/{language}/level")]
        public async Task<IActionResult> GetCodersByLanguage([FromQuery] string level, string language = "English")
        {
            try
            {
                var coders = await _coderRepository.GetCodersBylanguage(level);
                if (coders is null || !coders.Any())
                {
                    return NotFound("No hay coder con ese nivel de idioma.");
                }
                return Ok(coders);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                throw;
            }
        }
    }
}
