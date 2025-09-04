namespace redsocial.test
{
    public class PostTests
    {
        [Fact]
        public void CreatePost_WithValidData_ShouldCreateSuccessfully()
        {
            // GIVEN: Datos válidos para crear un post
            var id = "post-001";
            var authorId = "emp-001";
            var content = "This is my first post in the company social network!";

            // WHEN: Se crea un nuevo post
            var post = new Post(id, authorId, content);

            // THEN: El post se crea correctamente con los datos proporcionados
            Assert.NotNull(post);
            Assert.Equal(id, post.Id);
            Assert.Equal(authorId, post.AuthorId);
            Assert.Equal(content, post.Content);
            Assert.Equal(0, post.Likes);
        }

        [Fact]
        public void CreatePost_WithEmptyContent_ShouldThrowException()
        {
            // GIVEN: Contenido vacío para crear un post
            var id = "post-001";
            var authorId = "emp-001";
            var content = "";

            // WHEN: Se intenta crear un post con contenido vacío
            // THEN: Se debe lanzar una excepción
            Assert.Throws<ArgumentException>(() => new Post(id, authorId, content));
        }

        [Fact]
        public void CreatePost_WithNullAuthorId_ShouldThrowException()
        {
            // GIVEN: Un ID de autor nulo para crear un post
            var id = "post-001";
            string authorId = null;
            var content = "This is my post";

            // WHEN: Se intenta crear un post con ID de autor nulo
            // THEN: Se debe lanzar una excepción
            Assert.Throws<ArgumentNullException>(() => new Post(id, authorId, content));
        }

        [Fact]
        public void AddLike_ToExistingPost_ShouldIncrementLikes()
        {
            // GIVEN: Un post existente sin likes
            var post = new Post("post-001", "emp-001", "Great content!");
            var initialLikes = post.Likes;

            // WHEN: Se agrega un like al post
            post.AddLike();

            // THEN: El número de likes se incrementa en uno
            Assert.Equal(initialLikes + 1, post.Likes);
        }

        [Fact]
        public void AddMultipleLikes_ToExistingPost_ShouldIncrementCorrectly()
        {
            // GIVEN: Un post existente sin likes
            var post = new Post("post-001", "emp-001", "Amazing content!");
            var likesToAdd = 5;

            // WHEN: Se agregan múltiples likes al post
            for (int i = 0; i < likesToAdd; i++)
            {
                post.AddLike();
            }

            // THEN: El número de likes debe ser igual al número de likes agregados
            Assert.Equal(likesToAdd, post.Likes);
        }

        [Fact]
        public void RemoveLike_FromPostWithLikes_ShouldDecrementLikes()
        {
            // GIVEN: Un post existente con likes
            var post = new Post("post-001", "emp-001", "Interesting content!");
            post.AddLike();
            post.AddLike();
            var initialLikes = post.Likes;

            // WHEN: Se remueve un like del post
            post.RemoveLike();

            // THEN: El número de likes se decrementa en uno
            Assert.Equal(initialLikes - 1, post.Likes);
        }

        [Fact]
        public void RemoveLike_FromPostWithoutLikes_ShouldNotGoBelowZero()
        {
            // GIVEN: Un post existente sin likes
            var post = new Post("post-001", "emp-001", "Some content!");

            // WHEN: Se intenta remover un like de un post sin likes
            post.RemoveLike();

            // THEN: El número de likes no debe ser negativo
            Assert.True(post.Likes >= 0);
        }
    }
}
