using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FirstCargoApp.Models;
using FirstCargoApp.Helper;


namespace FirstCargoApp.Controllers
{
    public class VehiculeController : Controller
    {
        private FirstCargoDbEntities db = new FirstCargoDbEntities();

        // GET: /Vehicule/
        public async Task<ActionResult> Index(NotificationMessage.ManageMessageId? message)
        {

            ViewBag.StatusMessage =
                message == NotificationMessage.ManageMessageId.RecordSuccess ? @ViewResources.Resource.RecordSuccess
                : message == NotificationMessage.ManageMessageId.EditRecordSuccess ? @ViewResources.Resource.EditRecordSuccess
                : message == NotificationMessage.ManageMessageId.DeleteRecordSuccess ? @ViewResources.Resource.DeleteRecordSuccess
                : "";

            ViewBag.ReturnUrl = Url.Action("Vehicule");

            int id = Int32.Parse(User.Identity.GetUserName().Split('|')[1]);
            if (Convert.ToBoolean((User.Identity.GetUserName().Split('|')[2])))
                return View(await db.Vehicule.ToListAsync());

            
            return View(await db.Vehicule.Where(s => s.userID==id).ToListAsync());
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
        public async Task<ActionResult> Create([Bind(Include="vehiculeID,senderName,senderAdress,senderEmail,senderPhoneNumber,recieverName,recieverAdress,recieverEmail,recieverPhoneNumber,destination,price,paid,weight,height,length,depth,contentDescription,userID,vehiculeType,frameNumber")] Vehicule vehicule)
        {
            ViewBag.ReturnUrl = Url.Action("Vehicule");

            if (ModelState.IsValid)
            {
                vehicule.userID = Int32.Parse(User.Identity.GetUserName().Split('|')[1]);
                db.Vehicule.Add(vehicule);
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    // Todo Log the error
                }

                return RedirectToAction("Index", new { Message = NotificationMessage.ManageMessageId.RecordSuccess });
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
        public async Task<ActionResult> Edit([Bind(Include = "vehiculeID,senderName,senderAdress,senderEmail,senderPhoneNumber,recieverName,recieverAdress,recieverEmail,recieverPhoneNumber,destination,price,paid,weight,height,length,depth,contentDescription,userID,vehiculeType,frameNumber")] Vehicule vehicule)
        {
            ViewBag.ReturnUrl = Url.Action("Vehicule");

            if (ModelState.IsValid)
            {
                vehicule.userID = Int32.Parse(User.Identity.GetUserName().Split('|')[1]);
                db.Entry(vehicule).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    // Todo Log the error
                }

                return RedirectToAction("Index", new { Message = NotificationMessage.ManageMessageId.EditRecordSuccess });
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
            ViewBag.ReturnUrl = Url.Action("Vehicule");

            Vehicule vehicule = await db.Vehicule.FindAsync(id);

            db.Vehicule.Remove(vehicule);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                // Todo Log the error
            }

            return RedirectToAction("Index", new { Message = NotificationMessage.ManageMessageId.DeleteRecordSuccess });
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
