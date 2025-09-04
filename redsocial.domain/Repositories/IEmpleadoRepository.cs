using RedSocial.Domain.Entities;

namespace RedSocial.Domain.Repositories
{
    public interface IEmpleadoRepository
    {
        Task<Empleado?> GetByIdAsync(string id);
        Task<Empleado?> GetByEmailAsync(string email);
        Task<Empleado> SaveAsync(Empleado empleado);
        Task<List<Empleado>> GetAllAsync();
    }
}
