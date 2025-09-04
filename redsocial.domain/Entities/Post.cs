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
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            
            if (string.IsNullOrEmpty(authorId))
                throw new ArgumentNullException(nameof(authorId));
            
            if (string.IsNullOrEmpty(content))
                throw new ArgumentException("El contenido no puede estar vacío", nameof(content));

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
