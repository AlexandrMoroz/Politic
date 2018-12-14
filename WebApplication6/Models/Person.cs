using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Person
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Должно быть неменее 3 символов")]
        [MaxLength(20, ErrorMessage = "Отчество должна быть менее 20 символов")]
        public string Name { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Должно быть неменее 3 символов")]
        [MaxLength(20, ErrorMessage = "Отчество должна быть менее 20 символов")]
        public string Family { get; set; }
        [MinLength(2, ErrorMessage = "Должно быть неменее 3 символов")]
        [MaxLength(20, ErrorMessage = "Отчество должна быть менее 20 символов")]
        public string Surname { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Foto { get; set; }
        public int Rate { get; set; }
        [MaxLength(2000, ErrorMessage = "Тест болжен быть менее 2000 символов")]
        public string WayToPolitics { get; set; }
        public string SubmittedId { get; set; }
        public int CityId { get; set; }

        public virtual ApplicationUser Submitted { get; set; }
        public virtual Party Party { get; set; }
        public virtual City City { get; set; }
        public virtual Position Position { get; set; }
        public virtual ICollection<Post> Posts { get; set; } 
        public virtual ICollection<PeoplesUsers> PersonUsers { get; set; }
        public virtual ICollection<PeopleComment> Comments { get; set; }
    }

    
}