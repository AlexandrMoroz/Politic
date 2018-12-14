using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Tag
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id
            { get; set; }
        [Required]
        public virtual string Name
        { get; set; }

        public virtual ICollection<Post> Posts
        { get; set; }
        
    }
}