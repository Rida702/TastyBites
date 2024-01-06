using System.ComponentModel.DataAnnotations;

namespace OrderFoodApplication.Models
{
    public class RegisterViewModel
    {
        [Required]
        public int Name { get; set; }
        [Required]
        public int Address { get; set; }
        [Required, EmailAddress]
        public int Email { get; set; }
        [Required]
        public int Password { get; set; }
    }
}
