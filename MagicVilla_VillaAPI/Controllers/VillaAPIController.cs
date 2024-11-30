using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Data;
using MagicVilla_VillaAPI.Models.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController: Controller
    {
        private readonly ILogger<VillaAPIController> _logger;
        private readonly ILogging _customLogger;

        public VillaAPIController(ILogger<VillaAPIController>  logger, ILogging customLogging)
        {
            _customLogger = customLogging;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _customLogger.Log("Getting all villas", "");
            _logger.LogInformation("Getting all villas");
            return Ok(VillaStore.villaList);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(VillaDto), 200)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        public ActionResult<VillaDto> GetVillaById(int id) 
        {
            if(id == 0)
            {
                _customLogger.Log("Get Villa Error with Id", "error");
                _logger.LogError("Get Villa Error with Id" + id);
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                _customLogger.Log("Not Found with Id", "warning");
                return NotFound();
            }

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<VillaDto> CreateVilla([FromBody]VillaDto villaDto) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Already Exist");
                return BadRequest(ModelState);
            }

            if(villaDto == null)
            {
                return BadRequest(villaDto);
            }
            if(villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDto);

            return CreatedAtRoute("GetVilla", new { id = villaDto.Id }, villaDto);

        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id) 
        {
            if(id == 0)
            {
                return BadRequest();
            }
            
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (villa == null) 
            {
                return NotFound();
            }
            VillaStore.villaList.Remove(villa);

            return NoContent();

        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {
            if(villaDto == null || id != villaDto.Id)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            villa.Name = villaDto.Name;
            villa.Sqft = villaDto.Sqft;
            villa.Occupancy = villaDto.Occupancy;

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDto)
        {
            if(patchDto == null || id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if(villa == null)
            {
                return BadRequest();
            }
            patchDto.ApplyTo(villa, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();

        }

    }
}
