using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class TagsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        protected UserManager<ApplicationUser> UserManager { get; set; }
        public TagsController()
        {
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }
        // GET: Tags
        public async Task<ActionResult> Index()
        {
            return View(await db.Tags.ToListAsync());
        }

        // GET: Tags/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeAttribute(Roles = "Admin")]
        public async Task<JsonResult> Create(List<Tag> Tags)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in Tags)
                {
                    if (db.Tags.FirstOrDefault(x => x.Name == item.Name) == null)
                    {
                        db.Tags.Add(item);
                    }
                }
                await db.SaveChangesAsync();
                return Json(new { Ok = "ok" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { error ="Invalind model" }, JsonRequestBehavior.AllowGet);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<JsonResult> DeleteConfirmed(int id)
        {
            Tag tag = await db.Tags.FindAsync(id);
            db.Tags.Remove(tag);
            await db.SaveChangesAsync();
            return Json(new { Ok = "ok" }, JsonRequestBehavior.AllowGet);
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
