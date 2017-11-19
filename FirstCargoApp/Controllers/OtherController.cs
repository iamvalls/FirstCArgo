using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FirstCargoApp.Models;
using FirstCargoApp.Helper;

namespace FirstCargoApp.Controllers
{
    public class OtherController : Controller
    {
        private FirstCargoDbEntities db = new FirstCargoDbEntities();

        // GET: /Other/
        public async Task<ActionResult> Index(NotificationMessage.ManageMessageId? message)
        {

            ViewBag.StatusMessage =
                message == NotificationMessage.ManageMessageId.RecordSuccess ? @ViewResources.Resource.RecordSuccess
                : message == NotificationMessage.ManageMessageId.EditRecordSuccess ? @ViewResources.Resource.EditRecordSuccess
                : message == NotificationMessage.ManageMessageId.DeleteRecordSuccess ? @ViewResources.Resource.DeleteRecordSuccess
                : "";

            ViewBag.ReturnUrl = Url.Action("Other");

            int id = Int32.Parse(User.Identity.GetUserName().Split('|')[1]);
            if (Convert.ToBoolean((User.Identity.GetUserName().Split('|')[2])))
                return View(await db.Other.ToListAsync());
            return View(await db.Other.Where(s => s.userID == id).ToListAsync());
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
        public async Task<ActionResult> Create([Bind(Include = "otherID,otherType,senderName,senderAdress,senderEmail,senderPhoneNumber,recieverName,recieverAdress,recieverEmail,recieverPhoneNumber,destination,price,paid,weight,height,length,depth,contentDescription,userID")] Other other)
        {
            ViewBag.ReturnUrl = Url.Action("Other");

            if (ModelState.IsValid)
            {
                other.userID = Int32.Parse(User.Identity.GetUserName().Split('|')[1]);
                db.Other.Add(other);
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
        public async Task<ActionResult> Edit([Bind(Include = "otherID,otherType,senderName,senderAdress,senderEmailsenderPhoneNumber,recieverName,recieverAdress,recieverEmail,recieverPhoneNumber,destination,price,paid,weight,height,length,depth,contentDescription,userID")] Other other)
        {
            ViewBag.ReturnUrl = Url.Action("Other");

            if (ModelState.IsValid)
            {
                db.Entry(other).State = EntityState.Modified;
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
            ViewBag.ReturnUrl = Url.Action("Other");

            Other other = await db.Other.FindAsync(id);
            db.Other.Remove(other);
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
