using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumeroVillaController : ControllerBase
    {
        private readonly ILogger<NumeroVillaController> _logger;
        private readonly IVillaRepository _villaRepo;
        private readonly INumeroVillaRepository _numeroRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public NumeroVillaController(ILogger<NumeroVillaController> logger, INumeroVillaRepository numeroaRepo, IMapper mapper, IVillaRepository villaRepo)
        {
            _villaRepo = villaRepo;
            _numeroRepo = numeroaRepo;
            _logger = logger;
            _mapper = mapper;
            _response = new();
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetNumeroVillas()
        {
            try
            {
                //return new List<VillaDto>
                //{
                //    new VillaDto {Id=1,Nombre="Vista a la piscina"},
                //    new VillaDto{Id=2, Nombre="Vista a la playa"}
                //}; 
                _logger.LogInformation("Obteniendo numeros villas");
                //return Ok(VillaStore.villas);
                IEnumerable<NumeroVilla> Numerovillas = await _numeroRepo.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(Numerovillas);
                _response.statusCode = HttpStatusCode.OK;


                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessage = new List<string>() { ex.ToString()};                
            }

            return _response;

            
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("id:int",Name = "GetNumeroVilla")]
        public async Task<ActionResult<ApiResponse>>GetNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("id invalido, es menor o igual a cero");
                    _response.statusCode=HttpStatusCode.BadRequest;

                    return BadRequest(_response);
                }

                //var villa = VillaStore.villas.FirstOrDefault(x => x.Id == id);
                var numerovilla = await _numeroRepo.Obtener(x => x.VillaNo == id);

                if (numerovilla == null)
                {

                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<NumeroVillaDto>(numerovilla);
                _response.statusCode = HttpStatusCode.OK;



                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };

            }

            return _response;



        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> AddNumeroVilla([FromBody] NumeroVillaCreateDto villacreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _numeroRepo.Obtener(x => x.VillaNo == villacreate.VillaNo) != null)
                {
                    ModelState.AddModelError("NombreExiste", "Ya existe un numero villa");
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.Obtener(v => v.Id == villacreate.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForanea", "El id de la villa no existe!");
                    return BadRequest(ModelState);
                }

                if (villacreate == null)
                {
                    return BadRequest(villacreate);
                }

                NumeroVilla modelo = _mapper.Map<NumeroVilla>(villacreate);

                await _numeroRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.statusCode = HttpStatusCode.Created;
                // return Ok(villa);
                return CreatedAtRoute("GetNumeroVilla", new { id = modelo.VillaNo }, modelo);

                /*de volaaa a te tiene ete fullin*/
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var Numerovilla = await _numeroRepo.Obtener(x => x.VillaNo == id);

                if (Numerovilla == null)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

               await _numeroRepo.Remover(Numerovilla);

                _response.statusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex )
            {
                _response.IsExitoso = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };

            }

            return BadRequest(_response);

        }

        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditarNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto villaUpdateDto)
        {
            if (villaUpdateDto == null || id != villaUpdateDto.VillaNo)
            {
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }


            if(await _villaRepo.Obtener(v => v.Id == villaUpdateDto.VillaId) == null)
            {
                ModelState.AddModelError("ClaveFornaea", "El id de la villa no eixste");
                return BadRequest(ModelState);
            }

            NumeroVilla modelo = _mapper.Map<NumeroVilla>(villaUpdateDto);

            await _numeroRepo.Actualizar(modelo);

            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);

        }
    }
}
