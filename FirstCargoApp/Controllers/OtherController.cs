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
using System.Threading;
using System.Globalization;
using PagedList;

namespace FirstCargoApp.Controllers
{
    public class OtherController : Controller
    {
        private FirstCargoDbEntities db = new FirstCargoDbEntities();

        // GET: /Other/
        public async Task<ActionResult> Index(NotificationMessage.ManageMessageId? message, string sortOrder, string currentFilter, string searchString, int? page)
        {

            ViewBag.StatusMessage =
                message == NotificationMessage.ManageMessageId.RecordSuccess ? @ViewResources.Resource.RecordSuccess
                : message == NotificationMessage.ManageMessageId.EditRecordSuccess ? @ViewResources.Resource.EditRecordSuccess
                : message == NotificationMessage.ManageMessageId.DeleteRecordSuccess ? @ViewResources.Resource.DeleteRecordSuccess
                : message == NotificationMessage.ManageMessageId.NoEntryFound ? @ViewResources.Resource.NoEntryFound
                : "";
            // This is to show Notifications
            ViewBag.ReturnUrl = Url.Action("Other");
            // Get all entries from DB
            var others = await db.Other.ToListAsync();

            int id = Int32.Parse(User.Identity.GetUserName().Split('|')[1]);

            // Get Entry for a specific User
            if (!Convert.ToBoolean((User.Identity.GetUserName().Split('|')[2])))
                others = others.Where(s => s.userID == id).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                others = others.Where(s => s.senderName.Contains(searchString)
                                       || s.recieverName.Contains(searchString)).ToList();
            }

            // In case there is no entry
            if(others.Count ==0)
                return RedirectToAction("Index", new { Message = NotificationMessage.ManageMessageId.NoEntryFound });

            // Viewbag for Sort

            ViewBag.SenderNameSortParm = String.IsNullOrEmpty(sortOrder) ? "sender_name_desc" : "";
            ViewBag.RecieverNameSortParm = sortOrder == "reciever" ? "reciever_name_desc" : "reciever";
            ViewBag.OtherTypeSortParm = sortOrder == "other_type" ? "other_type_desc" : "other_type";
            ViewBag.SenderAdressSortParm = sortOrder == "sender_adress" ? "sender_adress_desc" : "sender_adress";
            ViewBag.SenderEmailSortParm = sortOrder == "sender_email" ? "sender_email_desc" : "sender_email";
            ViewBag.SenderPhoneNumberSortParm = sortOrder == "sender_phone_number" ? "sender_phone_number_desc" : "sender_phone_number";
            ViewBag.RecieverAdressSortParm = sortOrder == "reciever_adress" ? "reciever_adress_desc" : "reciever_adress";
            ViewBag.RecieverEmailSortParm = sortOrder == "reciever_email" ? "reciever_email_desc" : "reciever_email";
            ViewBag.RecieverPhoneNumberSortParm = sortOrder == "reciever_phone_number" ? "reciever_phone_number_desc" : "reciever_phone_number";
            ViewBag.DestinationSortParm = sortOrder == "destination" ? "destination_desc" : "destination";
            ViewBag.PriceSortParm = sortOrder == "price" ? "price_desc" : "price";
            ViewBag.PaidSortParm = sortOrder == "paid" ? "paid_desc" : "paid";

            // Pages
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            switch (sortOrder)
            {
                case "sender_name_desc":
                    others = others.OrderByDescending(s => s.senderName).ToList();
                    break;
                case "reciever":
                    others = others.OrderBy(s => s.recieverName).ToList();
                    break;
                case "reciever_name_desc":
                    others = others.OrderByDescending(s => s.recieverName).ToList();
                    break;
                case "other_type":
                    others = others.OrderBy(s => s.otherType).ToList();
                    break;
                case "other_type_desc":
                    others = others.OrderByDescending(s => s.otherType).ToList();
                    break;
                case "sender_adress":
                    others = others.OrderBy(s => s.senderAdress).ToList();
                    break;
                case "sender_adress_desc":
                    others = others.OrderByDescending(s => s.senderAdress).ToList();
                    break;
                case "sender_email":
                    others = others.OrderBy(s => s.senderEmail).ToList();
                    break;
                case "sender_email_desc":
                    others = others.OrderByDescending(s => s.senderEmail).ToList();
                    break;
                case "sender_phone_number":
                    others = others.OrderBy(s => s.senderPhoneNumber).ToList();
                    break;
                case "sender_phone_number_desc":
                    others = others.OrderByDescending(s => s.senderPhoneNumber).ToList();
                    break;
                case "reciever_adress":
                    others = others.OrderBy(s => s.recieverAdress).ToList();
                    break;
                case "reciever_adress_desc":
                    others = others.OrderByDescending(s => s.recieverAdress).ToList();
                    break;
                case "reciever_email":
                    others = others.OrderBy(s => s.recieverEmail).ToList();
                    break;
                case "reciever_email_desc":
                    others = others.OrderByDescending(s => s.recieverEmail).ToList();
                    break;
                case "reciever_phone_number":
                    others = others.OrderBy(s => s.recieverPhoneNumber).ToList();
                    break;
                case "reciever_phone_number_desc":
                    others = others.OrderByDescending(s => s.recieverPhoneNumber).ToList();
                    break;
                case "destination":
                    others = others.OrderBy(s => s.destination).ToList();
                    break;
                case "destination_desc":
                    others = others.OrderByDescending(s => s.destination).ToList();
                    break;
                case "price":
                    others = others.OrderBy(s => s.price).ToList();
                    break;
                case "price_desc":
                    others = others.OrderByDescending(s => s.price).ToList();
                    break;
                case "paid":
                    others = others.OrderBy(s => s.paid).ToList();
                    break;
                case "paid_desc":
                    others = others.OrderByDescending(s => s.paid).ToList();
                    break;
                default:
                    others = others.OrderBy(s => s.senderName).ToList();
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(others.ToPagedList(pageNumber, pageSize));
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

        //initilizing culture on controller initialization
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Session["CurrentCulture"] != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["CurrentCulture"].ToString());
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["CurrentCulture"].ToString());
                Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator = ".";
                Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberGroupSeparator = " ";
                Thread.CurrentThread.CurrentUICulture.NumberFormat.CurrencyDecimalSeparator = ".";
            }
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
