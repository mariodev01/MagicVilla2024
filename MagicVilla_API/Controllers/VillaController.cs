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
        private ApplicationDbContext _db;

        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db)
        {
            _db = db;
            _logger = logger;
        }
        [HttpGet]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            //return new List<VillaDto>
            //{
            //    new VillaDto {Id=1,Nombre="Vista a la piscina"},
            //    new VillaDto{Id=2, Nombre="Vista a la playa"}
            //}; 
            _logger.LogInformation("Obteniendo villas");
            //return Ok(VillaStore.villas);
            return Ok(_db.villas.ToList());
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("id:int",Name = "GetVilla")]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("id invalido, es menor o igual a cero");
                return BadRequest("Id menor o igual a cero");
            }

            //var villa = VillaStore.villas.FirstOrDefault(x => x.Id == id);
            var villa = _db.villas.FirstOrDefault(x=>x.Id == id);

            if (villa ==null)
            {
                _logger.LogError("No existe la villa con el id " + id);
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> AddVilla([FromBody] VillaDto villa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(_db.villas.FirstOrDefault(v=>v.Nombre.ToLower()== villa.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "Ya existe una villa con ese nombre");
                return BadRequest(ModelState);
            }

            if (villa == null)
            {
                return BadRequest(villa);
            }

            if (villa.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Villa modelo = new()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                Tarifa = villa.Tarifa,
                Ocupantes = villa.Ocupantes,
                MetrosCuadrados = villa.MetrosCuadrados,
                ImagenUrl = villa.ImagenUrl,
                Amenidad = villa.Amenidad
            };

            _db.villas.Add(modelo);
            _db.SaveChanges();

            // return Ok(villa);
            return CreatedAtRoute("GetVilla", new {id = villa.Id },villa);

            /*de volaaa a te tiene ete fullin*/
        }

        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {

            if(id == 0)
            {
                return BadRequest();
            }

            var villa = _db.villas.FirstOrDefault(v => v.Id == id);

            if(villa == null)
            {
                return NotFound(villa);
            }

            _db.villas.Remove(villa);
            _db.SaveChanges();

            return NoContent();

        }

        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult EditarVilla(int id, [FromBody] VillaDto villaDto)
        {
            if (villaDto == null || id != villaDto.Id)
            {
                return BadRequest();
            }

            Villa modelo = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                Tarifa = villaDto.Tarifa,
                Ocupantes = villaDto.Ocupantes,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                ImagenUrl = villaDto.ImagenUrl,
                Amenidad = villaDto.Amenidad
            };

            _db.villas.Update(modelo);
            _db.SaveChanges();

            return NoContent();

        }

        [HttpPatch("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult EditPatchVilla(int id, JsonPatchDocument<VillaDto> jsonPatch)
        {

            if(jsonPatch == null || id == 0)
            {
                return BadRequest();
            }

            var villa = _db.villas.AsNoTracking().FirstOrDefault(v=>v.Id==id);

            VillaDto villaDto = new()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                Tarifa = villa.Tarifa,
                Ocupantes = villa.Ocupantes,
                MetrosCuadrados = villa.MetrosCuadrados,
                ImagenUrl = villa.ImagenUrl,
                Amenidad = villa.Amenidad
            };



            jsonPatch.ApplyTo(villaDto,ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Villa modelo = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                Tarifa = villaDto.Tarifa,
                Ocupantes = villaDto.Ocupantes,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                ImagenUrl = villaDto.ImagenUrl,
                Amenidad = villaDto.Amenidad
            };

            _db.villas.Update(modelo);
            _db.SaveChanges();
            return NoContent();

        }
    }
}
