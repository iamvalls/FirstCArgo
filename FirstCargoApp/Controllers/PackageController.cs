using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FirstCargoApp.Models;
using FirstCargoApp.Helper;

namespace FirstCargoApp.Controllers
{
    public class PackageController : Controller
    {
        private FirstCargoDbEntities db = new FirstCargoDbEntities();

        // GET: /Package/
        public async Task<ActionResult> Index(NotificationMessage.ManageMessageId? message)
        {

            ViewBag.StatusMessage =
                message == NotificationMessage.ManageMessageId.RecordSuccess ? @ViewResources.Resource.RecordSuccess
                : message == NotificationMessage.ManageMessageId.EditRecordSuccess ? @ViewResources.Resource.EditRecordSuccess
                : message == NotificationMessage.ManageMessageId.DeleteRecordSuccess ? @ViewResources.Resource.DeleteRecordSuccess
                : "";

            ViewBag.ReturnUrl = Url.Action("Package");

            int id = Int32.Parse(User.Identity.GetUserName().Split('|')[1]);
            if (Convert.ToBoolean((User.Identity.GetUserName().Split('|')[2])))
                return View(await db.Package.ToListAsync());
            return View(await db.Package.Where(s => s.userID == id).ToListAsync());
        }

        // GET: /Package/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = await db.Package.FindAsync(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        // GET: /Package/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Package/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "packageID,packageType,senderName,senderAdress,senderEmail,senderPhoneNumber,recieverName,recieverAdress,recieverEmail,recieverPhoneNumber,destination,price,paid,weight,height,length,depth,contentDescription,userID")] Package package)
        {
            ViewBag.ReturnUrl = Url.Action("Package");

            if (ModelState.IsValid)
            {
                package.userID = Int32.Parse(User.Identity.GetUserName().Split('|')[1]);
                db.Package.Add(package);
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

            return View(package);
        }

        // GET: /Package/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = await db.Package.FindAsync(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        // POST: /Package/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "packageID,packageType,senderName,senderAdress,senderEmail,senderPhoneNumber,recieverName,recieverAdress,recieverEmail,recieverPhoneNumber,destination,price,paid,weight,height,length,depth,contentDescription,userID")] Package package)
        {
            ViewBag.ReturnUrl = Url.Action("Package");

            if (ModelState.IsValid)
            {
                package.userID = Int32.Parse(User.Identity.GetUserName().Split('|')[1]);
                db.Entry(package).State = EntityState.Modified;
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
            return View(package);
        }

        // GET: /Package/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = await db.Package.FindAsync(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            return View(package);
        }

        // POST: /Package/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ViewBag.ReturnUrl = Url.Action("Package");

            Package package = await db.Package.FindAsync(id);
            db.Package.Remove(package);
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
