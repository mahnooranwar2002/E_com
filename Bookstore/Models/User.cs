using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="name is not be null there")]
       

        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int status { get; set; }
        [Required]
        public int is_admin { get; set; }

    }
}
