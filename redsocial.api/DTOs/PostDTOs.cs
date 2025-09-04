namespace RedSocial.Api.DTOs
{
    public class CreatePostRequest
    {
        public string AuthorId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }

    public class CreatePostsBatchRequest
    {
        public List<CreatePostRequest> Posts { get; set; } = new List<CreatePostRequest>();
    }

    public class PostResponse
    {
        public string Id { get; set; } = string.Empty;
        public string AuthorId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Likes { get; set; }
    }

    public class AddLikeRequest
    {
        public string PostId { get; set; } = string.Empty;
    }
}
