﻿using AutoMapper;
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
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepository _villaRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public VillaController(ILogger<VillaController> logger, IVillaRepository villaRepo, IMapper mapper)
        {
            _villaRepo = villaRepo;  
            _logger = logger;
            _mapper = mapper;
            _response = new();
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetVillas()
        {
            try
            {
                //return new List<VillaDto>
                //{
                //    new VillaDto {Id=1,Nombre="Vista a la piscina"},
                //    new VillaDto{Id=2, Nombre="Vista a la playa"}
                //}; 
                _logger.LogInformation("Obteniendo villas");
                //return Ok(VillaStore.villas);
                IEnumerable<Villa> villas = await _villaRepo.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<VillaDto>>(villas);
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
        [HttpGet("id:int",Name = "GetVilla")]
        public async Task<ActionResult<ApiResponse>>GetVilla(int id)
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
                var villa = await _villaRepo.Obtener(x => x.Id == id);

                if (villa == null)
                {

                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<VillaDto>(villa);
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
        public async Task<ActionResult<ApiResponse>> AddVilla([FromBody] VillCreateaDto villacreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.Obtener(x => x.Nombre.ToLower() == villacreate.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "Ya existe una villa con ese nombre");
                    return BadRequest(ModelState);
                }

                if (villacreate == null)
                {
                    return BadRequest(villacreate);
                }

                Villa modelo = _mapper.Map<Villa>(villacreate);

                await _villaRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.statusCode = HttpStatusCode.Created;
                // return Ok(villa);
                return CreatedAtRoute("GetVilla", new { id = modelo.Id }, modelo);

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
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villa = await _villaRepo.Obtener(x => x.Id == id);

                if (villa == null)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _villaRepo.Remover(villa);

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
        public async Task<IActionResult> EditarVilla(int id, [FromBody] VillaUpdateDto villaUpdateDto)
        {
            if (villaUpdateDto == null || id != villaUpdateDto.Id)
            {
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Villa modelo = _mapper.Map<Villa>(villaUpdateDto);

            await _villaRepo.Actualizar(modelo);

            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);

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

            var villa = await _villaRepo.Obtener(v=>v.Id==id,tracked:false);

            VillaUpdateDto updateDto = _mapper.Map<VillaUpdateDto>(villa);

            jsonPatch.ApplyTo(updateDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Villa modelo = _mapper.Map<Villa>(updateDto);

            await _villaRepo.Actualizar(modelo);

            _response.statusCode=HttpStatusCode.NoContent;


            return Ok(_response);

        }
    }
}
