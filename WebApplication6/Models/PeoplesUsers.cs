using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class PeoplesUsers
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public Person Person{ get; set; }
        public bool Like { get; set; }
        public bool DisLike { get; set; }
    }
}