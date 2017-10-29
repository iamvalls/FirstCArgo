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
    public class PackageController : Controller
    {
        private FirstCargoDbEntities db = new FirstCargoDbEntities();

        // GET: /Package/
        public async Task<ActionResult> Index()
        {
            var packages = db.PACKAGES.Include(p => p.CATEGORIES);
            return View(await packages.ToListAsync());
        }

        // GET: /Package/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PACKAGES packages = await db.PACKAGES.FindAsync(id);
            if (packages == null)
            {
                return HttpNotFound();
            }
            return View(packages);
        }

        // GET: /Package/Create
        public ActionResult Create()
        {
            ViewBag.categoryID = new SelectList(db.CATEGORIES, "categoryID", "categoryName");
            return View();
        }

        // POST: /Package/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="packageID,categoryID")] PACKAGES packages)
        {
            if (ModelState.IsValid)
            {
                db.PACKAGES.Add(packages);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.categoryID = new SelectList(db.CATEGORIES, "categoryID", "categoryName", packages.categoryID);
            return View(packages);
        }

        // GET: /Package/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PACKAGES packages = await db.PACKAGES.FindAsync(id);
            if (packages == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoryID = new SelectList(db.CATEGORIES, "categoryID", "categoryName", packages.categoryID);
            return View(packages);
        }

        // POST: /Package/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="packageID,categoryID")] PACKAGES packages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(packages).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.categoryID = new SelectList(db.CATEGORIES, "categoryID", "categoryName", packages.categoryID);
            return View(packages);
        }

        // GET: /Package/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PACKAGES packages = await db.PACKAGES.FindAsync(id);
            if (packages == null)
            {
                return HttpNotFound();
            }
            return View(packages);
        }

        // POST: /Package/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PACKAGES packages = await db.PACKAGES.FindAsync(id);
            db.PACKAGES.Remove(packages);
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
