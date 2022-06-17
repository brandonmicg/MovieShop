using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    [Table("Movie")]
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(256)]
        [Required]
        public string Title { get; set; }

        [MaxLength(4096)]
        public string? Overview { get; set; }

        [MaxLength(512)]
        public string? Tagline { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Budget { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Revenue { get; set; }

        [MaxLength(2084)]
        public string? ImdbUrl { get; set; }

        [MaxLength(2084)]
        public string? TmdbUrl { get; set; }

        [MaxLength(2084)]
        public string? PosterUrl { get; set; }

        [MaxLength(2084)]
        public string? BackdropUrl { get; set; }

        [MaxLength(64)]
        public string? OriginalLanguage { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int? RunTime { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Price { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [MaxLength(4096)]
        public string? UpdatedBy { get; set; }

        [MaxLength(4096)]
        public string? CreatedBy { get; set; }


        public ICollection<MovieGenre> GenresOfMovies { get; set; }
        public ICollection<Trailer> Trailers { get; set; }
        public ICollection<MovieCast> CastOfMovies { get; set; }
        public ICollection<MovieCrew> CrewOfMovies { get; set; }
        public ICollection<Review> ReviewOfMovies { get; set; }
        public ICollection<Favorite> FavoritesOfMovies { get; set; }
        public ICollection<Purchase> PurchasesOfMovies { get; set; }
    }
}
