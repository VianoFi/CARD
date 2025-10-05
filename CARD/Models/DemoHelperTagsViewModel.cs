using System.ComponentModel.DataAnnotations;

namespace CARD.Models
{
    public class DemoHelperTagsViewModel
    {
        [Required(ErrorMessage = "Il nome è obbligatorio")]
        [Display(Name = "Nome Completo")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "L'email è obbligatoria")]
        [EmailAddress(ErrorMessage = "Email non valida")]
        [Display(Name = "Indirizzo Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Seleziona un paese")]
        [Display(Name = "Paese")]
        public string? Paese { get; set; }
    }
}