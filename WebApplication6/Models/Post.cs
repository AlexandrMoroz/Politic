using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Post
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int PostId { get; set; }
        [Required]
        public virtual string Title { get; set; }
        public virtual DateTime PostedOn { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual string PostFile { get; set; }
        public virtual string UserId { get; set; }
        public virtual int PersonId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<PostDescription> Description { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<PostComment> PostComment { get; set; }

    }
}