using System.ComponentModel.DataAnnotations;

namespace CardCollectionApp.Models
{
    public class MagicCard
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal Valore { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Z0-9\-]+$")]
        public string NumeroSerie { get; set; }

        [Required]
        [StringLength(100)]
        public string Espansione { get; set; }
    }
}
