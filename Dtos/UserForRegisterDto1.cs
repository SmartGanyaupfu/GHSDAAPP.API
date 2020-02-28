using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GHSDAAPP.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(16,MinimumLength =8,ErrorMessage ="Password must be between 8 and 16 characters")]
        public string Password { get; set; }
    }
}
