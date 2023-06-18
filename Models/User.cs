using System.ComponentModel.DataAnnotations;

namespace EComm.Models
{
    public class User
    {
        [Key]
        public int uid { get; set; }

        [Required]
        public string uname { get; set; }

        [Required]
        [EmailAddress]
        public string uemail { get; set; }

        [Required]
        public string umobile { get; set; }

        [Required]
        public string upassword { get; set; }

        public string? isAdmin { get; set; }
    }
}
