using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class PeopleComment: Comment
    {
        
        public virtual Person Person { get; set; }
    }
}