using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FirstCargoApp.Models;

namespace FirstCargoApp.Controllers
{
    public class OrtherController : Controller
    {
        private FirstCargoDbEntities db = new FirstCargoDbEntities();

        // GET: /Orther/
        public async Task<ActionResult> Index()
        {
            var others = db.OTHERS.Include(o => o.CATEGORIES);
            return View(await others.ToListAsync());
        }

        // GET: /Orther/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OTHERS others = await db.OTHERS.FindAsync(id);
            if (others == null)
            {
                return HttpNotFound();
            }
            return View(others);
        }

        // GET: /Orther/Create
        public ActionResult Create()
        {
            ViewBag.categoryID = new SelectList(db.CATEGORIES, "categoryID", "categoryName");
            return View();
        }

        // POST: /Orther/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="otherID,otherType,categoryID")] OTHERS others)
        {
            if (ModelState.IsValid)
            {
                db.OTHERS.Add(others);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.categoryID = new SelectList(db.CATEGORIES, "categoryID", "categoryName", others.categoryID);
            return View(others);
        }

        // GET: /Orther/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OTHERS others = await db.OTHERS.FindAsync(id);
            if (others == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoryID = new SelectList(db.CATEGORIES, "categoryID", "categoryName", others.categoryID);
            return View(others);
        }

        // POST: /Orther/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="otherID,otherType,categoryID")] OTHERS others)
        {
            if (ModelState.IsValid)
            {
                db.Entry(others).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.categoryID = new SelectList(db.CATEGORIES, "categoryID", "categoryName", others.categoryID);
            return View(others);
        }

        // GET: /Orther/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OTHERS others = await db.OTHERS.FindAsync(id);
            if (others == null)
            {
                return HttpNotFound();
            }
            return View(others);
        }

        // POST: /Orther/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OTHERS others = await db.OTHERS.FindAsync(id);
            db.OTHERS.Remove(others);
            await db.SaveChangesAsync();
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
