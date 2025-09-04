namespace RedSocial.Domain.Entities
{
    public class Empleado
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }

        public Empleado(string id, string name, string email)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID no puede estar vacío o contener solo espacios en blanco", nameof(id));
            
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre no puede estar vacío o contener solo espacios en blanco", nameof(name));
            
            if (email == null)
                throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                throw new ArgumentException("Email inválido", nameof(email));

            Id = id;
            Name = name;
            Email = email;
        }

        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();
            return trimmedEmail.Contains("@") && 
                   trimmedEmail.Contains(".") && 
                   !trimmedEmail.Contains(" ");
        }
    }
}
