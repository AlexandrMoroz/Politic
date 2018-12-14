using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class PostDescription
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Type { get; set; }
        [ForeignKey("Post")]
        public virtual int  PostId { get; set; } 
        public virtual Post Post { get; set; }
    }
}