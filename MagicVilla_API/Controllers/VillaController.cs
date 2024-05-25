using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            //return new List<VillaDto>
            //{
            //    new VillaDto {Id=1,Nombre="Vista a la piscina"},
            //    new VillaDto{Id=2, Nombre="Vista a la playa"}
            //}; 
            _logger.LogInformation("Obteniendo villas");
            //return Ok(VillaStore.villas);
            IEnumerable<Villa> villas = await _db.villas.ToListAsync();

            return Ok(_mapper.Map<IEnumerable<VillaDto>>(villas));
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("id:int",Name = "GetVilla")]
        public async Task<ActionResult<VillaDto>>GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("id invalido, es menor o igual a cero");
                return BadRequest("Id menor o igual a cero");
            }

            //var villa = VillaStore.villas.FirstOrDefault(x => x.Id == id);
            var villa = await _db.villas.FirstOrDefaultAsync(x=>x.Id == id);

            if (villa ==null)
            {
                _logger.LogError("No existe la villa con el id " + id);
                return NotFound();
            }
            return Ok(_mapper.Map<VillaDto>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDto>> AddVilla([FromBody] VillCreateaDto villacreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(await _db.villas.FirstOrDefaultAsync(v=>v.Nombre.ToLower()== villacreate.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "Ya existe una villa con ese nombre");
                return BadRequest(ModelState);
            }

            if (villacreate == null)
            {
                return BadRequest(villacreate);
            }

            Villa modelo = _mapper.Map<Villa>(villacreate);

            await _db.villas.AddAsync(modelo);
            await _db.SaveChangesAsync();

            // return Ok(villa);
            return CreatedAtRoute("GetVilla", new {id = modelo.Id },modelo);

            /*de volaaa a te tiene ete fullin*/
        }

        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var villa = await _db.villas.FirstOrDefaultAsync(v => v.Id == id);

            if(villa == null)
            {
                return NotFound(villa);
            }

            _db.villas.Remove(villa);
            await _db.SaveChangesAsync();

            return NoContent();

        }

        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditarVilla(int id, [FromBody] VillaUpdateDto villaUpdateDto)
        {
            if (villaUpdateDto == null || id != villaUpdateDto.Id)
            {
                return BadRequest();
            }

            Villa modelo = _mapper.Map<Villa>(villaUpdateDto);

            _db.villas.Update(modelo);
            await _db.SaveChangesAsync();

            return NoContent();

        }

        [HttpPatch("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditPatchVilla(int id, JsonPatchDocument<VillaUpdateDto> jsonPatch)
        {

            if(jsonPatch == null || id == 0)
            {
                return BadRequest();
            }

            var villa = await _db.villas.AsNoTracking().FirstOrDefaultAsync(v=>v.Id==id);

            VillaUpdateDto updateDto = _mapper.Map<VillaUpdateDto>(villa);

            jsonPatch.ApplyTo(updateDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Villa modelo = _mapper.Map<Villa>(updateDto);

            _db.villas.Update(modelo);
            await _db.SaveChangesAsync();
            return NoContent();

        }
    }
}
