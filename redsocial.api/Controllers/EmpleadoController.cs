using Microsoft.AspNetCore.Mvc;
using RedSocial.Api.DTOs;
using RedSocial.Application.Services;

namespace RedSocial.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController : ControllerBase
    {
        private readonly EmpleadoService _empleadoService;

        public EmpleadoController(EmpleadoService empleadoService)
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
        /// Crear múltiples empleados
        /// </summary>
        [HttpPost("batch")]
        public async Task<ActionResult<List<EmpleadoResponse>>> CreateMultipleEmpleados([FromBody] List<CreateEmpleadoRequest> requests)
        {
            var responses = new List<EmpleadoResponse>();
            var errors = new List<string>();

            foreach (var request in requests)
            {
                try
                {
                    var empleado = await _empleadoService.CreateEmpleado(request.Name, request.Email);
                    
                    responses.Add(new EmpleadoResponse
                    {
                        Id = empleado.Id,
                        Name = empleado.Name,
                        Email = empleado.Email
                    });
                }
                catch (InvalidOperationException ex)
                {
                    errors.Add($"Error creating employee '{request.Name}': {ex.Message}");
                }
                catch (ArgumentException ex)
                {
                    errors.Add($"Error creating employee '{request.Name}': {ex.Message}");
                }
            }

            if (errors.Any() && responses.Count == 0)
            {
                // Todos fallaron
                return BadRequest(new { message = "All employees failed to create", errors = errors });
            }
            
            if (errors.Any())
            {
                // Algunos fallaron
                return StatusCode(207, new { 
                    message = "Some employees created successfully, some failed",
                    created = responses,
                    errors = errors 
                });
            }

            // Todos exitosos
            return CreatedAtAction(nameof(CreateMultipleEmpleados), responses);
        }

        /// <summary>
        /// Obtener todos los empleados
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<EmpleadoResponse>>> GetAllEmpleados()
        {
            try
            {
                var empleados = await _empleadoService.GetAllEmpleados();
                
                var response = empleados.Select(e => new EmpleadoResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving employees", details = ex.Message });
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
