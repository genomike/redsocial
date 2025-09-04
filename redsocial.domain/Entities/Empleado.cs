namespace RedSocial.Domain.Entities
{
    public class Empleado
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }

        public Empleado(string id, string name, string email)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("El nombre no puede estar vacío", nameof(name));
            
            if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
                throw new ArgumentException("Email invalido", nameof(email));

            Id = id;
            Name = name;
            Email = email;
        }

        private bool IsValidEmail(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }
    }
}
