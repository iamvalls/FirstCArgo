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
    public class VehiculeController : Controller
    {
        private FirstCargoDbEntities db = new FirstCargoDbEntities();

        // GET: /Vehicule/
        public async Task<ActionResult> Index()
        {
            return View(await db.Vehicule.ToListAsync());
        }

        // GET: /Vehicule/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicule vehicule = await db.Vehicule.FindAsync(id);
            if (vehicule == null)
            {
                return HttpNotFound();
            }
            return View(vehicule);
        }

        // GET: /Vehicule/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Vehicule/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="vehiculeID,senderName,senderAdress,senderPhoneNumber,recieverName,recieverAdress,recieverPhoneNumber,destination,price,paid,weight,height,length,depth,contentDescription,userID,vehiculeType,frameNumber")] Vehicule vehicule)
        {
            if (ModelState.IsValid)
            {
                db.Vehicule.Add(vehicule);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(vehicule);
        }

        // GET: /Vehicule/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicule vehicule = await db.Vehicule.FindAsync(id);
            if (vehicule == null)
            {
                return HttpNotFound();
            }
            return View(vehicule);
        }

        // POST: /Vehicule/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="vehiculeID,senderName,senderAdress,senderPhoneNumber,recieverName,recieverAdress,recieverPhoneNumber,destination,price,paid,weight,height,length,depth,contentDescription,userID,vehiculeType,frameNumber")] Vehicule vehicule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicule).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vehicule);
        }

        // GET: /Vehicule/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicule vehicule = await db.Vehicule.FindAsync(id);
            if (vehicule == null)
            {
                return HttpNotFound();
            }
            return View(vehicule);
        }

        // POST: /Vehicule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Vehicule vehicule = await db.Vehicule.FindAsync(id);
            db.Vehicule.Remove(vehicule);
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
