using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Models.Tokens
{
    public class RefreshToken
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string token { get; set; }
        [Required]
        public int idUser { get; set; }
        [Required]
        public string role { get; set; }
    }
}
