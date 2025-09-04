using RedSocial.Domain.Entities;

namespace redsocial.test.domain
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

        [Fact]
        public void CreatePost_WithWhitespaceOnlyContent_ShouldThrowException()
        {
            // GIVEN: Contenido que solo contiene espacios en blanco
            var id = "post-008";
            var authorId = "emp-001";
            var content = "   ";

            // WHEN & THEN: Se debe lanzar una excepción
            Assert.ThrowsAny<ArgumentException>(() => new Post(id, authorId, content));
        }

        [Fact]
        public void CreatePost_WithWhitespaceOnlyId_ShouldThrowException()
        {
            // GIVEN: Un ID que solo contiene espacios en blanco
            var id = "   ";
            var authorId = "emp-001";
            var content = "Valid content";

            // WHEN & THEN: Se debe lanzar una excepción
            Assert.ThrowsAny<ArgumentException>(() => new Post(id, authorId, content));
        }

        [Fact]
        public void CreatePost_WithWhitespaceOnlyAuthorId_ShouldThrowException()
        {
            // GIVEN: Un authorId que solo contiene espacios en blanco
            var id = "post-009";
            var authorId = "   ";
            var content = "Valid content";

            // WHEN & THEN: Se debe lanzar una excepción
            Assert.ThrowsAny<ArgumentException>(() => new Post(id, authorId, content));
        }

        [Fact]
        public void CreatePost_WithVeryLongContent_ShouldCreateSuccessfully()
        {
            // GIVEN: Contenido muy largo pero válido
            var id = "post-010";
            var authorId = "emp-001";
            var content = new string('A', 2000); // Contenido de 2000 caracteres

            // WHEN: Se crea el post
            var post = new Post(id, authorId, content);

            // THEN: Se debe crear exitosamente
            Assert.NotNull(post);
            Assert.Equal(content, post.Content);
        }

        [Fact]
        public void AddLike_MultipleTimesSequentially_ShouldIncrementCorrectly()
        {
            // GIVEN: Un post existente
            var post = new Post("post-011", "emp-001", "Content for like testing");

            // WHEN: Se agregan múltiples likes secuencialmente
            for (int i = 1; i <= 10; i++)
            {
                post.AddLike();
                
                // THEN: El número de likes debe incrementar correctamente en cada iteración
                Assert.Equal(i, post.Likes);
            }
        }

        [Fact]
        public void RemoveLike_MultipleTimesSequentially_ShouldDecrementCorrectly()
        {
            // GIVEN: Un post con 10 likes
            var post = new Post("post-012", "emp-001", "Content for like removal testing");
            for (int i = 0; i < 10; i++)
            {
                post.AddLike();
            }

            // WHEN: Se remueven múltiples likes secuencialmente
            for (int i = 9; i >= 0; i--)
            {
                post.RemoveLike();
                
                // THEN: El número de likes debe decrementar correctamente en cada iteración
                Assert.Equal(i, post.Likes);
            }
        }

        [Fact]
        public void Post_LikesProperty_ShouldBeReadOnly()
        {
            // GIVEN: Un post existente
            var post = new Post("post-013", "emp-001", "Testing likes property");

            // WHEN: Se intenta acceder a la propiedad Likes
            var likesValue = post.Likes;

            // THEN: La propiedad debe ser accesible pero no debe tener setter público
            Assert.Equal(0, likesValue);
            
            // Verificar que no hay setter público disponible
            var likesProperty = typeof(Post).GetProperty("Likes");
            Assert.NotNull(likesProperty);
            Assert.True(likesProperty!.SetMethod == null || !likesProperty.SetMethod.IsPublic);
        }
    }
}
