namespace redsocial.test
{
    public class EmpleadoServiceTests
    {
        public class EmpleadoServiceTests
        {
            [Fact]
            public void CreateEmpleado_WithValidData_ShouldReturnNewEmpleado()
            {
                // GIVEN: Un servicio de empleados y datos válidos
                var empleadoService = new EmpleadoService();
                var name = "Jane Smith";
                var email = "jane.smith@company.com";

                // WHEN: Se crea un empleado a través del servicio
                var empleado = empleadoService.CreateEmpleado(name, email);

                // THEN: Se debe retornar un empleado válido con los datos correctos
                Assert.NotNull(empleado);
                Assert.Equal(name, empleado.Name);
                Assert.Equal(email, empleado.Email);
                Assert.NotNull(empleado.Id);
            }

            [Fact]
            public void CreateEmpleado_WithDuplicateEmail_ShouldThrowException()
            {
                // GIVEN: Un servicio de empleados con un empleado existente
                var empleadoService = new EmpleadoService();
                var email = "duplicate@company.com";
                empleadoService.CreateEmpleado("First User", email);

                // WHEN: Se intenta crear otro empleado con el mismo email
                // THEN: Se debe lanzar una excepción
                Assert.Throws<InvalidOperationException>(() =>
                    empleadoService.CreateEmpleado("Second User", email));
            }

            [Fact]
            public void GetEmpleadoById_WithValidId_ShouldReturnCorrectEmpleado()
            {
                // GIVEN: Un servicio de empleados con un empleado existente
                var empleadoService = new EmpleadoService();
                var expectedName = "Test Employee";
                var empleado = empleadoService.CreateEmpleado(expectedName, "test@company.com");

                // WHEN: Se busca el empleado por su ID
                var foundEmpleado = empleadoService.GetEmpleadoById(empleado.Id);

                // THEN: Se debe retornar el empleado correcto
                Assert.NotNull(foundEmpleado);
                Assert.Equal(empleado.Id, foundEmpleado.Id);
                Assert.Equal(expectedName, foundEmpleado.Name);
            }

            [Fact]
            public void GetEmpleadoById_WithInvalidId_ShouldReturnNull()
            {
                // GIVEN: Un servicio de empleados y un ID inexistente
                var empleadoService = new EmpleadoService();
                var invalidId = "non-existent-employee";

                // WHEN: Se busca un empleado con un ID inexistente
                var foundEmpleado = empleadoService.GetEmpleadoById(invalidId);

                // THEN: Se debe retornar null
                Assert.Null(foundEmpleado);
            }
        }
    }
}
