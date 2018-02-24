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
using System.Threading;
using System.Globalization;
using PagedList;


namespace FirstCargoApp.Controllers
{
    public class VehiculeController : Controller
    {
        private FirstCargoDbEntities db = new FirstCargoDbEntities();

        // GET: /Vehicule/
        public async Task<ActionResult> Index(NotificationMessage.ManageMessageId? message, string sortOrder, string currentFilter, string searchString, int? page)
        {

            ViewBag.StatusMessage =
                message == NotificationMessage.ManageMessageId.RecordSuccess ? @ViewResources.Resource.RecordSuccess
                : message == NotificationMessage.ManageMessageId.EditRecordSuccess ? @ViewResources.Resource.EditRecordSuccess
                : message == NotificationMessage.ManageMessageId.DeleteRecordSuccess ? @ViewResources.Resource.DeleteRecordSuccess
                : message == NotificationMessage.ManageMessageId.NoEntryFound ? @ViewResources.Resource.NoEntryFound
                : "";

            ViewBag.ReturnUrl = Url.Action("Vehicule");
            var vehicules = await db.Vehicule.ToListAsync();

            int id = Int32.Parse(User.Identity.GetUserName().Split('|')[1]);

            // Get Entry for a specific User
            if (!Convert.ToBoolean((User.Identity.GetUserName().Split('|')[2])))
                vehicules = vehicules.Where(s => s.userID == id).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                vehicules = vehicules.Where(s => s.senderName.Contains(searchString)
                                       || s.recieverName.Contains(searchString)).ToList();
            }
            // Show only the last 6 digit of the Frame number
            for (int i = 0; i < vehicules.Count(); i++)
            {
                if (vehicules[i].frameNumber.Length == 17)
                vehicules[i].frameNumber = vehicules[i].frameNumber.Substring(11, 6);
            }

            //// In case there is no entry
            //if (vehicules.Count == 0)
            //    return RedirectToAction("Index", new { Message = NotificationMessage.ManageMessageId.NoEntryFound });

            ViewBag.SenderNameSortParm = String.IsNullOrEmpty(sortOrder) ? "sender_name_desc" : "";
            ViewBag.OrderNumberSortParm = sortOrder == "order_number" ? "order_number_desc" : "order_number";
            ViewBag.CreatedDateSortParm = sortOrder == "created_date" ? "created_date_desc" : "created_date";
            ViewBag.RecieverNameSortParm = sortOrder == "reciever" ? "reciever_name_desc" : "reciever";
            ViewBag.VehiculeTypeSortParm = sortOrder == "vehicule_type" ? "vehicule_type_desc" : "vehicule_type";
            ViewBag.SenderAdressSortParm = sortOrder == "sender_adress" ? "sender_adress_desc" : "sender_adress";
            ViewBag.SenderEmailSortParm = sortOrder == "sender_email" ? "sender_email_desc" : "sender_email";
            ViewBag.SenderPhoneNumberSortParm = sortOrder == "sender_phone_number" ? "sender_phone_number_desc" : "sender_phone_number";
            ViewBag.RecieverAdressSortParm = sortOrder == "reciever_adress" ? "reciever_adress_desc" : "reciever_adress";
            ViewBag.RecieverEmailSortParm = sortOrder == "reciever_email" ? "reciever_email_desc" : "reciever_email";
            ViewBag.RecieverPhoneNumberSortParm = sortOrder == "reciever_phone_number" ? "reciever_phone_number_desc" : "reciever_phone_number";
            ViewBag.DestinationSortParm = sortOrder == "destination" ? "destination_desc" : "destination";
            ViewBag.PriceSortParm = sortOrder == "price" ? "price_desc" : "price";
            ViewBag.PaidSortParm = sortOrder == "paid" ? "paid_desc" : "paid";
            ViewBag.FrameNumberortParm = sortOrder == "frame_number" ? "frame_number_desc" : "frame_number";

            // Pages
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }


            switch (sortOrder)
            {
                case "order_number":
                    vehicules = vehicules.OrderByDescending(s => s.orderNumber).ToList();
                    break;
                case "order_number_desc":
                    vehicules = vehicules.OrderBy(s => s.orderNumber).ToList();
                    break;
                case "created_date":
                    vehicules = vehicules.OrderByDescending(s => s.createdDate).ToList();
                    break;
                case "created_date_desc":
                    vehicules = vehicules.OrderBy(s => s.createdDate).ToList();
                    break;
                case "sender_name_desc":
                    vehicules = vehicules.OrderByDescending(s => s.senderName).ToList();
                    break;
                case "reciever":
                    vehicules = vehicules.OrderBy(s => s.recieverName).ToList();
                    break;
                case "reciever_name_desc":
                    vehicules = vehicules.OrderByDescending(s => s.recieverName).ToList();
                    break;
                case "vehicule_type":
                    vehicules = vehicules.OrderBy(s => s.vehiculeType).ToList();
                    break;
                case "vehicule_type_desc":
                    vehicules = vehicules.OrderByDescending(s => s.vehiculeType).ToList();
                    break;
                case "sender_adress":
                    vehicules = vehicules.OrderBy(s => s.senderAdress).ToList();
                    break;
                case "sender_adress_desc":
                    vehicules = vehicules.OrderByDescending(s => s.senderAdress).ToList();
                    break;
                case "sender_email":
                    vehicules = vehicules.OrderBy(s => s.senderEmail).ToList();
                    break;
                case "sender_email_desc":
                    vehicules = vehicules.OrderByDescending(s => s.senderEmail).ToList();
                    break;
                case "sender_phone_number":
                    vehicules = vehicules.OrderBy(s => s.senderPhoneNumber).ToList();
                    break;
                case "sender_phone_number_desc":
                    vehicules = vehicules.OrderByDescending(s => s.senderPhoneNumber).ToList();
                    break;
                case "reciever_adress":
                    vehicules = vehicules.OrderBy(s => s.recieverAdress).ToList();
                    break;
                case "reciever_adress_desc":
                    vehicules = vehicules.OrderByDescending(s => s.recieverAdress).ToList();
                    break;
                case "reciever_email":
                    vehicules = vehicules.OrderBy(s => s.recieverEmail).ToList();
                    break;
                case "reciever_email_desc":
                    vehicules = vehicules.OrderByDescending(s => s.recieverEmail).ToList();
                    break;
                case "reciever_phone_number":
                    vehicules = vehicules.OrderBy(s => s.recieverPhoneNumber).ToList();
                    break;
                case "reciever_phone_number_desc":
                    vehicules = vehicules.OrderByDescending(s => s.recieverPhoneNumber).ToList();
                    break;
                case "destination":
                    vehicules = vehicules.OrderBy(s => s.destination).ToList();
                    break;
                case "destination_desc":
                    vehicules = vehicules.OrderByDescending(s => s.destination).ToList();
                    break;
                case "price":
                    vehicules = vehicules.OrderBy(s => s.price).ToList();
                    break;
                case "price_desc":
                    vehicules = vehicules.OrderByDescending(s => s.price).ToList();
                    break;
                case "paid":
                    vehicules = vehicules.OrderBy(s => s.paid).ToList();
                    break;
                case "paid_desc":
                    vehicules = vehicules.OrderByDescending(s => s.paid).ToList();
                    break;
                case "frame_number":
                    vehicules = vehicules.OrderBy(s => s.frameNumber).ToList();
                    break;
                case "frame_number_desc":
                    vehicules = vehicules.OrderByDescending(s => s.frameNumber).ToList();
                    break;
                default:
                    vehicules = vehicules.OrderBy(s => s.senderName).ToList();
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(vehicules.ToPagedList(pageNumber, pageSize));
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
        public async Task<ActionResult> Create([Bind(Include="vehiculeID,senderName,senderAdress,senderEmail,senderPhoneNumber,recieverName,recieverAdress,recieverEmail,recieverPhoneNumber,destination,price,paid,alreadyPaid,paidRest,weight,height,length,depth,userID,vehiculeType,frameNumber")] Vehicule vehicule)
        {
            ViewBag.ReturnUrl = Url.Action("Vehicule");

            if (ModelState.IsValid)
            {
                vehicule.userID = Int32.Parse(User.Identity.GetUserName().Split('|')[1]);
                vehicule.createdDate = DateTime.Now;
                vehicule.paidRest = vehicule.price - vehicule.alreadyPaid;

                var orderNumber = db.Vehicule.Where(p => p.vehiculeID != 0 && p.vehiculeID == db.Vehicule.Max(r => r.vehiculeID)).SingleOrDefault();
                int getNumber = Int32.Parse(orderNumber.orderNumber.Remove(0, 2)) + 1;
                vehicule.orderNumber = "VH" + getNumber;

                if (vehicule.senderEmail.Equals(""))
                {
                    vehicule.senderEmail = "first-cargo-mannheim@outlook.de";
                }
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
        public async Task<ActionResult> Edit([Bind(Include = "vehiculeID,senderName,senderAdress,senderEmail,senderPhoneNumber,recieverName,recieverAdress,recieverEmail,recieverPhoneNumber,destination,price,paid,alreadyPaid,paidRest,weight,height,length,depth,userID,vehiculeType,frameNumber")] Vehicule vehicule)
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


        // GET: /Vehicule/PrintOrder/5
        public async Task<ActionResult> PrintOrder(int? id)
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

        // POST: /Vehicule/PrintOrder/5
        [HttpPost, ActionName("PrintOrder")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PrintOrderConfirmed(int id)
        {
            ViewBag.ReturnUrl = Url.Action("Vehicule");

            Vehicule vehicule = await db.Vehicule.FindAsync(id);
            //db.Other.Remove(other);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                // Todo Log the error
            }
            return RedirectToAction("Index", new { Message = NotificationMessage.ManageMessageId.PrintOrderSuccess });
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
