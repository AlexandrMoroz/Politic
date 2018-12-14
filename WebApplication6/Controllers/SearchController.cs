using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;
using PagedList.Mvc;
using PagedList;
namespace WebApplication6.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        protected UserManager<ApplicationUser> UserManager { get; set; }
        public SearchController()
        {
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }
        // GET: Search
        public ActionResult PeopleSearch(SearchStringViewModel model, int? page)
        {
            if (ModelState.IsValid)
            {
                int pageSize = 8;
                int pageNumber = (page ?? 1);
                var Peoples = db.Persons.Where(x => x.Family.Contains(model.SearchString) ||
                                                    x.Name.Contains(model.SearchString) ||
                                                    x.Surname.Contains(model.SearchString)
                                                    ).OrderBy(x=>x.Family);
                ViewBag.returnAction = "PeopleSearch";
                
                if (User.Identity.IsAuthenticated)
                {
                    var Userid = User.Identity.GetUserId();
                    ViewBag.flags = db.PeopleUsers.Where(x => x.User.Id == Userid).ToList();
                }
                return View("Index", Peoples.ToPagedList(pageNumber, pageSize));

            }
            return View("Index",new List<Person>().ToPagedList(8,8));
        }

        public ActionResult PositionSearch(int id, int? page)
        {
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            ViewBag.returnAction = "PositionSearch";
            if (User.Identity.IsAuthenticated)
            {
                var Userid = User.Identity.GetUserId();
                ViewBag.flags = db.PeopleUsers.Where(x => x.User.Id == Userid).ToList();
            }
            return View("Index", db.Persons.Where(x => x.Position.Id == id)
                                           .OrderBy(x => x.Family)
                                           .ToPagedList(pageNumber, pageSize));
        }
        public ActionResult PartySearch(int id, int? page)
        {
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            ViewBag.returnAction = "PartySearch";
            if (User.Identity.IsAuthenticated)
            {
                var Userid = User.Identity.GetUserId();
                ViewBag.flags = db.PeopleUsers.Where(x => x.User.Id == Userid).ToList();
            }
            return View("Index", db.Persons.Where(x => x.Party.Id == id)
                                           .OrderBy(x => x.Family)
                                           .ToPagedList(pageNumber, pageSize)
                                           );
        }
        public ActionResult TagsSearch(int id, int? page)
        {
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            var Tag = db.Tags.First(x => x.Id == id);
            ViewBag.returnAction = "TagsSearch";
            if (User.Identity.IsAuthenticated)
            {
                var Userid = User.Identity.GetUserId();
                ViewBag.flags = db.PeopleUsers.Where(x => x.User.Id == Userid).ToList();
            }
            return View("PostsSearch", db.Posts.Where(x => x.Tags.Contains(Tag))
                                               .OrderBy(x => x.PostedOn)
                                               .ToPagedList(pageNumber, pageSize));
        }
    }

}