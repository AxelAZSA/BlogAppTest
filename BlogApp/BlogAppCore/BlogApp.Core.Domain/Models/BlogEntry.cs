using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Domain.Models
{
    public class BlogEntry
    {
        [Key]
        public int IdEntry { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public int idUser { get; set; }
        public int idCategory { get; set; }
        public string ImageUrl { get; set; }
    }

}
