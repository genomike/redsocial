namespace redsocial.test.domain
{
    public class EmpleadoTests
    {
        [Fact]
        public void CreateEmpleado_WithValidData_ShouldCreateSuccessfully()
        {
            // GIVEN: Datos válidos para crear un empleado
            var id = "emp-001";
            var name = "John Doe";
            var email = "john.doe@company.com";

            // WHEN: Se crea un nuevo empleado
            var empleado = new Empleado(id, name, email);

            // THEN: El empleado se crea correctamente con los datos proporcionados
            Assert.NotNull(empleado);
            Assert.Equal(id, empleado.Id);
            Assert.Equal(name, empleado.Name);
            Assert.Equal(email, empleado.Email);
        }

        [Fact]
        public void CreateEmpleado_WithNullId_ShouldThrowException()
        {
            // GIVEN: Un ID nulo para crear un empleado
            string id = null;
            var name = "John Doe";
            var email = "john.doe@company.com";

            // WHEN: Se intenta crear un empleado con ID nulo
            // THEN: Se debe lanzar una excepción
            Assert.Throws<ArgumentNullException>(() => new Empleado(id, name, email));
        }

        [Fact]
        public void CreateEmpleado_WithEmptyName_ShouldThrowException()
        {
            // GIVEN: Un nombre vacío para crear un empleado
            var id = "emp-001";
            var name = "";
            var email = "john.doe@company.com";

            // WHEN: Se intenta crear un empleado con nombre vacío
            // THEN: Se debe lanzar una excepción
            Assert.Throws<ArgumentException>(() => new Empleado(id, name, email));
        }

        [Fact]
        public void CreateEmpleado_WithInvalidEmail_ShouldThrowException()
        {
            // GIVEN: Un email inválido para crear un empleado
            var id = "emp-001";
            var name = "John Doe";
            var email = "invalid-email";

            // WHEN: Se intenta crear un empleado con email inválido
            // THEN: Se debe lanzar una excepción
            Assert.Throws<ArgumentException>(() => new Empleado(id, name, email));
        }
    }
}