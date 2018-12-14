using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class PeopleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }
        public PeopleController()
        {
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }

        // GET: People
        public ActionResult Index()
        {
            var AllowPositions = new List<string>() {"Народный депутат", "Министр", "Помошник Министра", "Мэр", "МВД" };
            var positions  = db.Positions.Where(x => AllowPositions.Contains(x.Name)).ToList();
            var PeoplesPerPos = new Dictionary<Position, List<Person>>();
            foreach (var item in positions)
            {
                var temp = db.Persons.Where(x => x.Position.Id == item.Id)
                                    .OrderBy(x => x.Rate)
                                    .Take(15)
                                    .ToList();

                PeoplesPerPos.Add(item, temp);
            }
            if (User.Identity.IsAuthenticated)
            {
                var id = User.Identity.GetUserId();
                ViewBag.flags = db.PeopleUsers.Where(x => x.User.Id == id).ToList();
            }
            return View(new PersonIndexViewModel() { PeoplePerPosition = PeoplesPerPos });

        }

        public ActionResult Top(int page = 1)
        {
            int pageSize = 8; // количество объектов на страницу
            IEnumerable<Person> Persons = db.Persons.OrderBy(x => x.Rate).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            if (User.Identity.IsAuthenticated)
            {
                Session["MainImg"] = UserManager.FindById(User.Identity.GetUserId()).MainImage;
                var id = User.Identity.GetUserId();
                ViewBag.flags = db.PeopleUsers.Where(x => x.User.Id == id).ToList();
                return View("Top", Persons);
            }
            return View("Top", Persons);
        }
        // GET: People/Details/5
        public ActionResult Details(int id)
        {

            Person person = db.Persons.FirstOrDefault(x=>x.Id == id);
            if (person == null)
            {
                throw new HttpException(404, "NotFound");
            }
            var temp=db.PeopleUsers.FirstOrDefault(x => x.Person.Id == id);
            if (temp != null) {
                ViewBag.Like = db.PeopleUsers.Where(x => x.Person.Id == id).Sum(x => x.Like ? 1 : 0);
                ViewBag.DisLike = db.PeopleUsers.Where(x => x.Person.Id == id).Sum(x => x.DisLike ? 1 : 0);
            }
            else
            {
                ViewBag.Like =0;
                ViewBag.DisLike = 0;
            }
            ViewBag.CriminalId = db.Tags.FirstOrDefault(x => x.Name == "криминал").Id;
            ViewBag.GoodJobId = db.Tags.FirstOrDefault(x => x.Name == "хорошие дела").Id;
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                ViewBag.UserName = UserManager.Users.First(x => x.Id == userId).Name;
            }
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {

            ViewBag.Cities = db.Cities.Select(x => new { Id = x.Id, Name = x.Name }).ToList();
            ViewBag.Parties = db.Parties.Select(x => new { Id = x.Id, Name = x.Name }).ToList();
            return View();
        }

        // POST: People/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Family,Surname,Date,Foto,Rate")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Persons.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new HttpException(404, "Not found");
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                throw new HttpException(404, "Not found");
            }
            return View(person);
        }

        // POST: People/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Family,Surname,Date,Foto,Rate")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new HttpException(404, "Not found");
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                throw new HttpException(404, "Not found");
            }
            return View(person);
        }
        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.Persons.Find(id);
            db.Persons.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Auth]
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult Like(int id)
        {
            var people = db.Persons.Where(x => x.Id == id).FirstOrDefault();
            if (people == null)
            {
                return Json(new { error = "404" }, JsonRequestBehavior.AllowGet);
            }
            else if (!User.Identity.IsAuthenticated)
            {
                return Json(new { error = "login" }, JsonRequestBehavior.AllowGet);
            }
            var userId = User.Identity.GetUserId();
            var UserTemp = db.PeopleUsers.FirstOrDefault(x => x.User.Id == userId && x.Person.Id == id);
            if (UserTemp != null)
            {
                if (UserTemp.Like)
                {
                    return Json(new { rate = people.Rate, flag = 1, id = id }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (UserTemp.DisLike)
                    {
                        people.Rate += 1;
                        UserTemp.Like = false;
                        UserTemp.DisLike = false;
                        db.SaveChanges();
                        return Json(new { rate = people.Rate, flag = 0, id = id }, JsonRequestBehavior.AllowGet);

                    }
                    people.Rate += 1;
                    UserTemp.Like = true;
                    UserTemp.DisLike = false;
                    db.SaveChanges();
                    return Json(new { rate = people.Rate, flag = 1, id = id }, JsonRequestBehavior.AllowGet);
                }
            }
            people.Rate += 1;
            var user = UserManager.FindById(userId);
            db.PeopleUsers.Add(new PeoplesUsers() { Person = people, User = user, Like = true, DisLike = false });
            db.SaveChanges();
            return Json(new { rate = people.Rate, flag = 1, id = id }, JsonRequestBehavior.AllowGet);
        }
        [Auth]
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult DisLike(int id)
        {
            var people = db.Persons.Where(x => x.Id == id).FirstOrDefault();
            if (people == null)
            {
                return Json(new { error = "404" }, JsonRequestBehavior.AllowGet);
            }
            else if (!User.Identity.IsAuthenticated)
            {
                return Json(new { error = "Login" },JsonRequestBehavior.AllowGet);
            }
            var userId = User.Identity.GetUserId();
            var UserTemp = db.PeopleUsers.FirstOrDefault(x => x.User.Id == userId && x.Person.Id == id);

            if (UserTemp != null)
            {
                if (UserTemp.DisLike)
                {
                    return Json(new { rate = people.Rate, flag = 2, id = id }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (UserTemp.Like)
                    {
                        people.Rate -= 1;
                        UserTemp.Like = false;
                        UserTemp.DisLike = false;
                        db.SaveChanges();

                        return Json(new { rate = people.Rate, flag = 0, id = id }, JsonRequestBehavior.AllowGet);
                    }
                    people.Rate -= 1;
                    UserTemp.Like = false;
                    UserTemp.DisLike = true;
                    db.SaveChanges();

                    return Json(new { rate = people.Rate, flag = 2, id = id }, JsonRequestBehavior.AllowGet);
                }
            }
            people.Rate -= 1;
            var user = UserManager.FindById(userId);
            db.PeopleUsers.Add(new PeoplesUsers() { Person = people, User = user, DisLike = true, Like = false });
            db.SaveChanges();
            return Json(new { rate = people.Rate, flag = 2, id = id }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Info(int id) {
            Person person = db.Persons.FirstOrDefault(x => x.Id == id);
            if (person == null)
            {
                throw new HttpException(404, "Not found");
            }

            if (person.WayToPolitics != null)
            {
                if (person.WayToPolitics == "")
                {
                    ViewBag.WayToPolit = "";
                }
                else
                {
                    ViewBag.WayToPolit = person.WayToPolitics.Split('\n');
                }
            }
            else
            {
                ViewBag.WayToPolit = "";
            }
            return View(person);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
