using Microsoft.EntityFrameworkCore;
using RedSocial.Domain.Entities;
using RedSocial.Infrastructure.Data;
using RedSocial.Infrastructure.Repositories;

namespace redsocial.test.infrastructure
{
    public class EmpleadoRepositoryTests : IDisposable
    {
        private readonly RedSocialDbContext _context;
        private readonly EmpleadoRepository _repository;

        public EmpleadoRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<RedSocialDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new RedSocialDbContext(options);
            _repository = new EmpleadoRepository(_context);
        }

        [Fact]
        public async Task SaveAsync_WithValidEmpleado_ShouldSaveToDatabase()
        {
            // GIVEN: Un empleado válido
            var empleado = new Empleado("emp-001", "Juan Pérez", "juan@company.com");

            // WHEN: Se guarda en el repositorio
            var savedEmpleado = await _repository.SaveAsync(empleado);

            // THEN: El empleado debe estar guardado correctamente
            Assert.NotNull(savedEmpleado);
            Assert.Equal(empleado.Name, savedEmpleado.Name);
            Assert.Equal(empleado.Email, savedEmpleado.Email);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnEmpleado()
        {
            // GIVEN: Un empleado existente en la base de datos
            var empleado = new Empleado("emp-002", "Ana García", "ana@company.com");
            await _repository.SaveAsync(empleado);

            // WHEN: Se busca por ID
            var foundEmpleado = await _repository.GetByIdAsync(empleado.Id);

            // THEN: Se debe retornar el empleado correcto
            Assert.NotNull(foundEmpleado);
            Assert.Equal(empleado.Id, foundEmpleado.Id);
            Assert.Equal(empleado.Name, foundEmpleado.Name);
            Assert.Equal(empleado.Email, foundEmpleado.Email);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // GIVEN: Un ID inexistente
            var invalidId = "non-existent-id";

            // WHEN: Se busca por ID inexistente
            var foundEmpleado = await _repository.GetByIdAsync(invalidId);

            // THEN: Se debe retornar null
            Assert.Null(foundEmpleado);
        }

        [Fact]
        public async Task GetByEmailAsync_WithValidEmail_ShouldReturnEmpleado()
        {
            // GIVEN: Un empleado existente en la base de datos
            var empleado = new Empleado("emp-003", "Carlos López", "carlos@company.com");
            await _repository.SaveAsync(empleado);

            // WHEN: Se busca por email
            var foundEmpleado = await _repository.GetByEmailAsync(empleado.Email);

            // THEN: Se debe retornar el empleado correcto
            Assert.NotNull(foundEmpleado);
            Assert.Equal(empleado.Id, foundEmpleado.Id);
            Assert.Equal(empleado.Name, foundEmpleado.Name);
            Assert.Equal(empleado.Email, foundEmpleado.Email);
        }

        [Fact]
        public async Task GetByEmailAsync_WithInvalidEmail_ShouldReturnNull()
        {
            // GIVEN: Un email inexistente
            var invalidEmail = "nonexistent@company.com";

            // WHEN: Se busca por email inexistente
            var foundEmpleado = await _repository.GetByEmailAsync(invalidEmail);

            // THEN: Se debe retornar null
            Assert.Null(foundEmpleado);
        }

        [Fact]
        public async Task GetAllAsync_WhenEmpleadosExist_ShouldReturnAllEmpleados()
        {
            // GIVEN: Varios empleados en la base de datos
            var empleado1 = new Empleado("emp-004", "María Torres", "maria@company.com");
            var empleado2 = new Empleado("emp-005", "Diego Sánchez", "diego@company.com");
            
            await _repository.SaveAsync(empleado1);
            await _repository.SaveAsync(empleado2);

            // WHEN: Se solicitan todos los empleados
            var allEmpleados = await _repository.GetAllAsync();

            // THEN: Se deben retornar todos los empleados
            Assert.NotNull(allEmpleados);
            Assert.True(allEmpleados.Count >= 2);
            Assert.Contains(allEmpleados, e => e.Name == empleado1.Name);
            Assert.Contains(allEmpleados, e => e.Name == empleado2.Name);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoEmpleadosExist_ShouldReturnEmptyList()
        {
            // GIVEN: Base de datos vacía
            // (No se agregan empleados)

            // WHEN: Se solicitan todos los empleados
            var allEmpleados = await _repository.GetAllAsync();

            // THEN: Se debe retornar una lista vacía
            Assert.NotNull(allEmpleados);
            Assert.Empty(allEmpleados);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
