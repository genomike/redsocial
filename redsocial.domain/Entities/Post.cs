namespace RedSocial.Domain.Entities
{
    public class Post
    {
        public string Id { get; private set; }
        public string AuthorId { get; private set; }
        public string Content { get; private set; }
        public int Likes { get; private set; }

        public Post(string id, string authorId, string content)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID no puede estar vacío o contener solo espacios en blanco", nameof(id));
            
            if (authorId == null)
                throw new ArgumentNullException(nameof(authorId));
            if (string.IsNullOrWhiteSpace(authorId))
                throw new ArgumentException("El ID del autor no puede estar vacío o contener solo espacios en blanco", nameof(authorId));
            
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("El contenido no puede estar vacío o contener solo espacios en blanco", nameof(content));

            Id = id;
            AuthorId = authorId;
            Content = content;
            Likes = 0;
        }

        public void AddLike()
        {
            Likes++;
        }

        public void RemoveLike()
        {
            if (Likes > 0)
                Likes--;
        }
    }
}
