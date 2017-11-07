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
    public class OtherController : Controller
    {
        private FirstCargoDbEntities db = new FirstCargoDbEntities();

        // GET: /Other/
        public async Task<ActionResult> Index()
        {
            return View(await db.Other.ToListAsync());
        }

        // GET: /Other/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Other other = await db.Other.FindAsync(id);
            if (other == null)
            {
                return HttpNotFound();
            }
            return View(other);
        }

        // GET: /Other/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Other/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="otherID,otherType,senderName,senderAdress,senderPhoneNumber,recieverName,recieverAdress,recieverPhoneNumber,destination,price,paid,weight,height,length,depth,contentDescription,userID")] Other other)
        {
            if (ModelState.IsValid)
            {
                db.Other.Add(other);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(other);
        }

        // GET: /Other/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Other other = await db.Other.FindAsync(id);
            if (other == null)
            {
                return HttpNotFound();
            }
            return View(other);
        }

        // POST: /Other/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="otherID,otherType,senderName,senderAdress,senderPhoneNumber,recieverName,recieverAdress,recieverPhoneNumber,destination,price,paid,weight,height,length,depth,contentDescription,userID")] Other other)
        {
            if (ModelState.IsValid)
            {
                db.Entry(other).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(other);
        }

        // GET: /Other/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Other other = await db.Other.FindAsync(id);
            if (other == null)
            {
                return HttpNotFound();
            }
            return View(other);
        }

        // POST: /Other/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Other other = await db.Other.FindAsync(id);
            db.Other.Remove(other);
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
