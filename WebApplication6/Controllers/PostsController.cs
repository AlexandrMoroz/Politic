using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{

    [Auth]
    public class PostsController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();
        protected UserManager<ApplicationUser> UserManager { get; set; }
        public PostsController()
        {
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }

        // GET: Posts 
        // Tag had to state criminal 
        // Tag
        [AllowAnonymous]
        public ActionResult Index(int Id, int TagId, int page = 1 )
        {
            var tag = db.Tags.FirstOrDefault(x => x.Id == TagId);
            if (tag == null)
            {
                return HttpNotFound();
            }
            int pageSize = 2; // количество объектов на страницу
            IEnumerable<Post> Posts = db.Posts.Where(x => x.PersonId == Id && x.Tags.FirstOrDefault(y => y.Id == tag.Id) != null).
                                                       OrderBy(x => x.PostId).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.postType = tag.Name;
            ViewBag.TagId = TagId;

            return View(Posts);
        }

        // GET: Posts 
        [AllowAnonymous]
        public ActionResult Details(int id, int TagId, string message="")
        {
            if (message != "")
            {
                ViewBag.message = message;
            }   
            var post = db.Posts.FirstOrDefault(x => x.PostId == id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.postType =  db.Tags.FirstOrDefault(x => x.Id == TagId).Name;
            ViewBag.personId = post.PersonId;
          
            return View(post);
        }
        // GET: Posts/Create
        public ActionResult Create(int TagId, int peopleId)
        {
            var postType = db.Tags.Where(x => x.Id == TagId).FirstOrDefault();
            ViewBag.PersonId = peopleId;
            if (postType!=null)
            {
                ViewBag.TypeOfPost = postType.Id;
            }
            else
            {
                return HttpNotFound();
            }

            return View("Create");
        }

        // POST: Posts/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        // добавить создание файла по дате поста и папку картинки в ней

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult Create(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var postType = db.Tags.FirstOrDefault(x => x.Id == model.PostType);
                Post postTemp;
                var orderList = new List<PostDescription>();
                
                foreach (var item in model.Order)
                {
                    var type = Regex.Replace(item.Key, "[0-9]", string.Empty);
                    var temp = new PostDescription() { Type = type, Content = item.Value };
                    orderList.Add(temp);
                }

                var tagsTemp = new List<Tag>();
                tagsTemp.Add(postType);
                var Tags = Regex.Replace(model.Tags, "[^a-zа-яA-ZА-Я0-9,.\\s]", string.Empty);
                foreach (var item in Tags.Split(','))
                {
                    tagsTemp.Add(new Tag() { Name = item });
                }
                if (model.Files != null)
                {
                    string FileFolderPath;
                    //проверяем дерикторию создвем если нет 
                    FileFolderPath = DateTime.Now.ToString("ddMMyyyyHHmmss");
                    if (!Directory.Exists(Server.MapPath(FileFolderPath)))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/PostContent/" + FileFolderPath));
                    }

                    //сохраняем файлы
                    foreach (HttpPostedFileBase file in model.Files)
                    {
                        if (file.ContentLength > 0)
                        {
                            file.SaveAs(Server.MapPath("~/Content/PostContent/" + FileFolderPath + "/" + file.FileName));
                        }
                    }
                    postTemp = new Post()
                    {
                        Title = model.Title,
                        UserId = User.Identity.GetUserId(),
                        PersonId = model.PersonId,
                        PostedOn = DateTime.Now,
                        Description = orderList,
                        Tags = tagsTemp,
                        PostFile = FileFolderPath
                    };
                }
                else
                {
                    postTemp = new Post()
                    {
                        Title = model.Title,
                        UserId = User.Identity.GetUserId(),
                        PersonId = model.PersonId,
                        PostedOn = DateTime.Now,
                        Description = orderList,
                        Tags = tagsTemp,
                    };
                }

                foreach (var item in tagsTemp)
                {
                    if (db.Tags.FirstOrDefault(x=>x.Name==item.Name)==null)
                    {
                        db.Tags.Add(item);
                    }
                }
                db.Posts.Add(postTemp);
                db.SaveChanges();
               
                return Json(new {postTemp.PostId, TagId = model.PostType,  message = "Пост успешно добавлен" }, JsonRequestBehavior.AllowGet);
            }

            return ModelState.JsonValidation();
        }
        
        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            if (post.UserId!=User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
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
