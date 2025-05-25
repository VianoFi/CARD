using System.ComponentModel.DataAnnotations;

namespace CARD.Models
{
    public class MagicCard
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Nome { get; set; }

        [Required, StringLength(50)]
        public string Espansione { get; set; }

        [Required, RegularExpression(@"^[0-9]{1,3}$")]
        public string NumeroSerie { get; set; }

        [Range(0, 30000)]
        public decimal Valore { get; set; }
    }
}

