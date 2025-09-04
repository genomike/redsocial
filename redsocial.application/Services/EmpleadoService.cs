using RedSocial.Domain.Entities;
using RedSocial.Domain.Repositories;

namespace RedSocial.Application.Services
{
    public class EmpleadoService
    {
        private readonly IEmpleadoRepository _empleadoRepository;

        public EmpleadoService(IEmpleadoRepository empleadoRepository)
        {
            _empleadoRepository = empleadoRepository;
        }

        // Constructor para tests (sin DI)
        public EmpleadoService()
        {
            _empleadoRepository = new InMemoryEmpleadoRepository();
        }

        public async Task<Empleado> CreateEmpleado(string name, string email)
        {
            var existingEmpleado = await _empleadoRepository.GetByEmailAsync(email);
            if (existingEmpleado != null)
                throw new InvalidOperationException("Email already exists");

            var id = Guid.NewGuid().ToString();
            var empleado = new Empleado(id, name, email);
            
            return await _empleadoRepository.SaveAsync(empleado);
        }

        public async Task<Empleado?> GetEmpleadoById(string id)
        {
            return await _empleadoRepository.GetByIdAsync(id);
        }

        public async Task<List<Empleado>> GetAllEmpleados()
        {
            return await _empleadoRepository.GetAllAsync();
        }
    }

    // Implementación en memoria para tests
    internal class InMemoryEmpleadoRepository : IEmpleadoRepository
    {
        private readonly List<Empleado> _empleados = new();

        public Task<Empleado?> GetByIdAsync(string id)
        {
            var empleado = _empleados.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(empleado);
        }

        public Task<Empleado?> GetByEmailAsync(string email)
        {
            var empleado = _empleados.FirstOrDefault(e => e.Email == email);
            return Task.FromResult(empleado);
        }

        public Task<Empleado> SaveAsync(Empleado empleado)
        {
            _empleados.Add(empleado);
            return Task.FromResult(empleado);
        }

        public Task<List<Empleado>> GetAllAsync()
        {
            return Task.FromResult(_empleados.ToList());
        }
    }
}
