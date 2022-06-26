using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    [Table("Purchase")]
    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid PurchaseNumber { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public DateTime PurchaseDateTime { get; set; }

        [Required]
        public int MovieId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }
    }
}
