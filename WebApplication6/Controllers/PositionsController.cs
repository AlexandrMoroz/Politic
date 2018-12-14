using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication6.Models;
using PagedList.Mvc;
using PagedList;

namespace WebApplication6.Controllers
{
    public class PositionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }
        public PositionsController()
        {
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }
        // GET: Positions
        public ActionResult Index(int id, int? page)
        {
            int pageSize =  8;
            int pageNumber = (page ?? 1);
            if (User.Identity.IsAuthenticated)
            {
                var UserId = User.Identity.GetUserId();
                ViewBag.flags = db.PeopleUsers.Where(x => x.User.Id == UserId).ToList();
            }
            return View(db.Persons.OrderBy(x=>x.Id).ToPagedList(pageNumber, pageSize));
        }
    }
}