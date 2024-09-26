using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Domain.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string confirmPassword { get; set; }
    }
}
