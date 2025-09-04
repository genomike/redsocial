using Microsoft.EntityFrameworkCore;
using RedSocial.Domain.Entities;

namespace RedSocial.Infrastructure.Data
{
    public class RedSocialDbContext : DbContext
    {
        public RedSocialDbContext(DbContextOptions<RedSocialDbContext> options) : base(options)
        {
        }

        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Empleado
            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(50);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(150).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configuración de Post
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasMaxLength(50);
                entity.Property(p => p.AuthorId).HasMaxLength(50).IsRequired();
                entity.Property(p => p.Content).HasMaxLength(1000).IsRequired();
                entity.Property(p => p.Likes).HasDefaultValue(0);
            });
        }
    }
}
