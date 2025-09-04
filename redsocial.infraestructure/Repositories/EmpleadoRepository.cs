using Microsoft.EntityFrameworkCore;
using RedSocial.Domain.Entities;
using RedSocial.Domain.Repositories;
using RedSocial.Infrastructure.Data;

namespace RedSocial.Infrastructure.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly RedSocialDbContext _context;

        public EmpleadoRepository(RedSocialDbContext context)
        {
            _context = context;
        }

        public async Task<Empleado?> GetByIdAsync(string id)
        {
            return await _context.Empleados.FindAsync(id);
        }

        public async Task<Empleado?> GetByEmailAsync(string email)
        {
            return await _context.Empleados
                .FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<Empleado> SaveAsync(Empleado empleado)
        {
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();
            return empleado;
        }

        public async Task<List<Empleado>> GetAllAsync()
        {
            return await _context.Empleados.ToListAsync();
        }
    }
}
