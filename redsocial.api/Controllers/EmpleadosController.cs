using Microsoft.AspNetCore.Mvc;
using RedSocial.Api.DTOs;
using RedSocial.Application.Services;

namespace RedSocial.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadosController : ControllerBase
    {
        private readonly EmpleadoService _empleadoService;

        public EmpleadosController(EmpleadoService empleadoService)
        {
            _empleadoService = empleadoService;
        }

        /// <summary>
        /// Crear un nuevo empleado
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<EmpleadoResponse>> CreateEmpleado([FromBody] CreateEmpleadoRequest request)
        {
            try
            {
                var empleado = await _empleadoService.CreateEmpleado(request.Name, request.Email);
                
                var response = new EmpleadoResponse
                {
                    Id = empleado.Id,
                    Name = empleado.Name,
                    Email = empleado.Email
                };

                return CreatedAtAction(nameof(GetEmpleadoById), new { id = empleado.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtener empleado por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpleadoResponse>> GetEmpleadoById(string id)
        {
            var empleado = await _empleadoService.GetEmpleadoById(id);
            
            if (empleado == null)
            {
                return NotFound(new { message = $"Empleado with ID {id} not found" });
            }

            var response = new EmpleadoResponse
            {
                Id = empleado.Id,
                Name = empleado.Name,
                Email = empleado.Email
            };

            return Ok(response);
        }
    }
}
