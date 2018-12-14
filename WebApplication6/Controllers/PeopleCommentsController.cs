using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication6.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace WebApplication6.Controllers
{
    [Auth]
    public class PeopleCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        protected UserManager<ApplicationUser> UserManager { get; set; }
        public PeopleCommentsController()
        {
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }
        // GET: Comments
        public ActionResult Index(int id, int ParentId = 0)
        {
            if (ParentId == 0)
            {
                var coments = db.PeopleComments.Where(x => x.PersonId == id && x.ParentId == null).ToList();

                ViewBag.personId = id;
                ViewBag.ChildFlag = 1;
                return View(coments);
            }
            else
            {
                var coments = db.PeopleComments.Where(x => x.PersonId == id && x.ParentId == ParentId).ToList();

                ViewBag.personId = id;
                ViewBag.ChildFlag = 0;
                return View(coments);
            }
        }

        // GET: Comments/Create
        public ActionResult Create(int id, int ParentId=0)
        {
                ViewBag.ParentId = ParentId;
                ViewBag.PersonId = id;
                return View();
        }

        // POST: Comments/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public  JsonResult Create([Bind(Include = "Text,ParentId,PersonId")] PeopleComment comment)
        {
            if (ModelState.IsValid)
            {
                PeopleComment com;
                comment.Text = Regex.Replace(comment.Text,
                                @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)",
                                "<a target='_blank' href='$1'>$1</a>");
                if (comment.ParentId == 0)
                {
                    var c = db.PeopleComments.Where(x => x.PersonId == comment.PersonId).ToList();
                    if (c.Any())
                    {
                        com = new PeopleComment()
                        {
                            Text = comment.Text,
                            Datetime = DateTime.Now,
                            User = UserManager.FindById(User.Identity.GetUserId()),
                            PersonId = comment.PersonId,
                            ParentId = null,
                        };
                        db.PeopleComments.Add(com);
                         db.SaveChanges();
                        //wrap all url in text to html links

                        return new JsonResult
                        {
                            Data = new
                            {
                                success = true,
                                ischild = false,
                                view = this.RenderPartialView("OnCreateResult", com)
                            },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        com = new PeopleComment()
                        {
                            Text = comment.Text,
                            Datetime = DateTime.Now,
                            User = UserManager.FindById(User.Identity.GetUserId()),
                            PersonId = comment.PersonId,
                            ParentId = null,
                        };
                        db.PeopleComments.Add(com);
                        db.SaveChanges();

                        var coments = new List<PeopleComment>();
                        coments.Add(com);
                        ViewBag.personId = comment.PersonId;
                        ViewBag.ChildFlag = 0;

                        return new JsonResult
                        {
                            Data = new
                            {
                                success = true,
                                ischild = false,
                                view = this.RenderPartialView("Index", coments)
                            },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    

                }
                else
                {
                 
                        com = new PeopleComment()
                        {
                            Text = comment.Text,
                            Datetime = DateTime.Now,
                            User = UserManager.FindById(User.Identity.GetUserId()),
                            PersonId = comment.PersonId,
                            ParentId = comment.ParentId,
                        };
                        var commentTemp =  db.PeopleComments.Find(comment.ParentId);
                        if (commentTemp==null)
                        {
                            return Json(new { error = "404" }, JsonRequestBehavior.AllowGet);
                        }
                        commentTemp.Childrens.Add(com);
                         db.SaveChanges();

                    var coments = db.PeopleComments.Where(x => x.PersonId == comment.PersonId && x.ParentId == comment.ParentId).ToList();
                    ViewBag.personId = comment.PersonId;
                    ViewBag.ChildFlag = 0;

                    return new JsonResult
                    {
                        Data =  new {
                            success = true,
                            ischild = true,
                            view = this.RenderPartialView("Index", coments)
                    },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                

                
            }

            return Json(new { error="404" }, JsonRequestBehavior.AllowGet);
        }
             
        public async Task<JsonResult> Delete(int id)
        {
            PeopleComment comment = await db.PeopleComments.FindAsync(id);
            if (comment == null)
            {
                return Json(new { error = "404" }, JsonRequestBehavior.AllowGet);
            }
            if (comment.UserId != User.Identity.GetUserId())
            {
                return Json(new { error = "Это не ваш коментарий" }, JsonRequestBehavior.AllowGet);
            }
            if (comment.Childrens.Any())
            {
                return Json(new { success=false, error = "Коментарий не может быть удаленна, так как содержит ответы" }, JsonRequestBehavior.AllowGet);
            }
            db.PeopleComments.Remove(comment);
            await db.SaveChangesAsync();
            return Json(Url.RouteUrl(new { Action = "Index", Controller = "PeopleComments", id = comment.PersonId, comment.ParentId }), JsonRequestBehavior.AllowGet);

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
