using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class MovieShopDbContext : DbContext
    {
        public MovieShopDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<Trailer> Trailers { get; set; }
        public DbSet<Cast> Casts { get; set; }
        public DbSet<MovieCast> MovieCasts { get; set; }
        public DbSet<Crew> Crew { get; set; }
        public DbSet<MovieCrew> MovieCrews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Purchase> Purchases { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieGenre>(ConfigMovieGenre);
            modelBuilder.Entity<MovieCast>(ConfigMovieCast);
            modelBuilder.Entity<MovieCrew>(ConfigMovieCrew);
            modelBuilder.Entity<UserRole>(ConfigUserRole);
            modelBuilder.Entity<Review>(ConfigReview);

        }

        private void ConfigMovieGenre(EntityTypeBuilder<MovieGenre> builder)
        {
            builder.HasKey(x => new {x.MovieId, x.GenreId});
        }

        private void ConfigMovieCast(EntityTypeBuilder<MovieCast> builder)
        {
            builder.HasKey(x => new { x.MovieId, x.CastId });
        }

        private void ConfigMovieCrew(EntityTypeBuilder<MovieCrew> builder)
        {
            builder.HasKey(x => new { x.MovieId, x.CrewId });
        }

        private void ConfigUserRole(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(x => new { x.UserId, x.RoleId });
        }

        private void ConfigReview(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(x => new { x.MovieId, x.UserId });
        }
    }
}
