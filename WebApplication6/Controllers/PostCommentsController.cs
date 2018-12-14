using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication6.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WebApplication6.Controllers
{
    [Auth]
    public class PostCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public PostCommentsController()
        {
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }
        public ActionResult Index(int id, int ParentId = 0)
        {
            if (ParentId == 0)
            {
                var coments = db.PostComments.Where(x => x.PersonId == id && x.ParentId == null).ToList();

                ViewBag.personId = id;
                ViewBag.ChildFlag = 1;
                return View(coments);
            }
            else
            {
                var coments = db.PostComments.Where(x => x.PersonId == id && x.ParentId == ParentId).ToList();

                ViewBag.personId = id;
                ViewBag.ChildFlag = 0;
                return View(coments);
            }
        }

        // GET: PostComments/Create
        public ActionResult Create(int id, int ParentId = 0)
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
        public ActionResult Create([Bind(Include = "Text,ParentId,PersonId")] PostComment comment)
        {
            if (ModelState.IsValid)
            {
                PostComment com;
                comment.Text = Regex.Replace(comment.Text,
                                @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)",
                                "<a target='_blank' href='$1'>$1</a>");
                if (comment.ParentId == 0)
                {
                    var c = db.PostComments.Where(x => x.PersonId == comment.PersonId).ToList();
                    if (c.Any())
                    {
                        com = new PostComment()
                        {
                            Text = comment.Text,
                            Datetime = DateTime.Now,
                            User = UserManager.FindById(User.Identity.GetUserId()),
                            PersonId = comment.PersonId,
                            ParentId = null,
                        };
                        db.PostComments.Add(com);
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
                        com = new PostComment()
                        {
                            Text = comment.Text,
                            Datetime = DateTime.Now,
                            User = UserManager.FindById(User.Identity.GetUserId()),
                            PersonId = comment.PersonId,
                            ParentId = null,
                        };
                        db.PostComments.Add(com);
                        db.SaveChanges();

                        var coments = new List<PostComment>();
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
                    com = new PostComment()
                    {
                        Text = comment.Text,
                        Datetime = DateTime.Now,
                        User = UserManager.FindById(User.Identity.GetUserId()),
                        PersonId = comment.PersonId,
                        ParentId = comment.ParentId,
                    };
                    var commentTemp = db.PostComments.Find(comment.ParentId);
                    if (commentTemp == null)
                    {
                        return Json(new { error = "404" }, JsonRequestBehavior.AllowGet);
                    }
                    commentTemp.Childrens.Add(com);
                    db.SaveChanges();

                    var coments = db.PostComments.Where(x => x.PersonId == comment.PersonId && x.ParentId == comment.ParentId).ToList();
                    ViewBag.personId = comment.PersonId;
                    ViewBag.ChildFlag = 0;

                    return new JsonResult
                    {
                        Data = new
                        {
                            success = true,
                            ischild = true,
                            view = this.RenderPartialView("Index", coments)
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }



            }

            return Json(new { error = "404" }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { error = "404" }, JsonRequestBehavior.AllowGet);
            }
            PostComment comment = await db.PostComments.FindAsync(id);
            if (comment == null)
            {
                return Json(new { error = "404" }, JsonRequestBehavior.AllowGet);
            }
            if (comment.UserId != User.Identity.GetUserId())
            {
                return Json(new { error = "Not your comment" }, JsonRequestBehavior.AllowGet);
            }
            if (comment.Childrens.Any())
            {
                return Json(new { success = false, error = "Коментарий не может быть удаленна, так как содержит ответы" }, JsonRequestBehavior.AllowGet);
            }
            db.PostComments.Remove(comment);
            await db.SaveChangesAsync();
            return Json(Url.RouteUrl(new { Action = "Index", Controller = "PostComments", id = comment.PersonId, comment.ParentId }), JsonRequestBehavior.AllowGet);
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
