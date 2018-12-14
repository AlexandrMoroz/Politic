using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApplication6.Models
{
    public abstract class Comment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Comment_Id { get; set; }
        [Required]
        public virtual  string Text { get; set; }
        public virtual DateTime Datetime { get; set; }
        public virtual string UserId { get; set; }
        public virtual int PersonId { get; set; }
        [ForeignKey("Parent")]
        public virtual int? ParentId { get; set; }
        /// <summary>
        /// comit user
        /// </summary>
        public virtual ApplicationUser User { get; set; }
        public virtual Comment Parent { get; set; }
        public virtual ICollection<Comment> Childrens { get; set; }

    }
}