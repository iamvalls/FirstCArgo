using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FirstCargoApp.Models;

namespace FirstCargoApp.Controllers
{
    public class VehiculeController : Controller
    {
        private FirstCargoDbEntities db = new FirstCargoDbEntities();

        // GET: /Vehicule/
        public ActionResult Vehicule()
        {
            var vehicules = db.VEHICULES.Include(v => v.CATEGORIES);
            return View(vehicules.ToList());
        }

        // GET: /Vehicule/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VEHICULES vehicules = db.VEHICULES.Find(id);
            if (vehicules == null)
            {
                return HttpNotFound();
            }
            return View(vehicules);
        }

        // GET: /Vehicule/Create
        public ActionResult Create()
        {
            ViewBag.categoryID = new SelectList(db.CATEGORIES, "categoryID", "categoryName");
            return View();
        }

        // POST: /Vehicule/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="vehiculeID,vehiculeType,frameNumber,categoryID")] VEHICULES vehicules)
        {
            if (ModelState.IsValid)
            {
                db.VEHICULES.Add(vehicules);
                db.SaveChanges();
                return RedirectToAction("Vehicule","Vehicule");
            }

            ViewBag.categoryID = new SelectList(db.CATEGORIES, "categoryID", "categoryName", vehicules.categoryID);
            return View(vehicules);
        }

        // GET: /Vehicule/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VEHICULES vehicules = db.VEHICULES.Find(id);
            if (vehicules == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoryID = new SelectList(db.CATEGORIES, "categoryID", "categoryName", vehicules.categoryID);
            return View(vehicules);
        }

        // POST: /Vehicule/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="vehiculeID,vehiculeType,frameNumber,categoryID")] VEHICULES vehicules)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicules).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.categoryID = new SelectList(db.CATEGORIES, "categoryID", "categoryName", vehicules.categoryID);
            return View(vehicules);
        }

        // GET: /Vehicule/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VEHICULES vehicules = db.VEHICULES.Find(id);
            if (vehicules == null)
            {
                return HttpNotFound();
            }
            return View(vehicules);
        }

        // POST: /Vehicule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VEHICULES vehicules = db.VEHICULES.Find(id);
            db.VEHICULES.Remove(vehicules);
            db.SaveChanges();
            return RedirectToAction("Vehicule","Vehicule");
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
