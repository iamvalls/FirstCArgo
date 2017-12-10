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
using System.Threading;
using System.Globalization;
using PagedList;

namespace FirstCargoApp.Controllers
{
    public class PackageController : Controller
    {
        private FirstCargoDbEntities db = new FirstCargoDbEntities();

        // GET: /Package/
        public async Task<ActionResult> Index(NotificationMessage.ManageMessageId? message, string sortOrder, string currentFilter, string searchString, int? page)
        {

            ViewBag.StatusMessage =
                message == NotificationMessage.ManageMessageId.RecordSuccess ? @ViewResources.Resource.RecordSuccess
                : message == NotificationMessage.ManageMessageId.EditRecordSuccess ? @ViewResources.Resource.EditRecordSuccess
                : message == NotificationMessage.ManageMessageId.DeleteRecordSuccess ? @ViewResources.Resource.DeleteRecordSuccess
                : message == NotificationMessage.ManageMessageId.NoEntryFound ? @ViewResources.Resource.NoEntryFound
                : "";

            ViewBag.ReturnUrl = Url.Action("Package");
            var packages = await db.Package.ToListAsync();

            int id = Int32.Parse(User.Identity.GetUserName().Split('|')[1]);
            // Get Entry for a specific User
            if (!Convert.ToBoolean((User.Identity.GetUserName().Split('|')[2])))
                packages = db.Package.Where(s => s.userID == id).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                packages = packages.Where(s => s.senderName.Contains(searchString)
                                       || s.recieverName.Contains(searchString)).ToList();
            }

            //// In case there is no entry
            //if (packages.Count == 0)
            //    return RedirectToAction("Index", new { Message = NotificationMessage.ManageMessageId.NoEntryFound });


            ViewBag.SenderNameSortParm = String.IsNullOrEmpty(sortOrder) ? "sender_name_desc" : "";
            ViewBag.RecieverNameSortParm = sortOrder == "reciever" ? "reciever_name_desc" : "reciever";
            ViewBag.PackageTypeSortParm = sortOrder == "package_type" ? "package_type_desc" : "package_type";
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
            

            switch (sortOrder)
            {
                case "sender_name_desc":
                    packages = packages.OrderByDescending(s => s.senderName).ToList();
                    break;
                case "reciever":
                    packages = packages.OrderBy(s => s.recieverName).ToList();
                    break;
                case "reciever_name_desc":
                    packages = packages.OrderByDescending(s => s.recieverName).ToList();
                    break;
                case "package_type":
                    packages = packages.OrderBy(s => s.packageType).ToList();
                    break;
                case "package_type_desc":
                    packages = packages.OrderByDescending(s => s.packageType).ToList();
                    break;
                case "sender_adress":
                    packages = packages.OrderBy(s => s.senderAdress).ToList();
                    break;
                case "sender_adress_desc":
                    packages = packages.OrderByDescending(s => s.senderAdress).ToList();
                    break;
                case "sender_email":
                    packages = packages.OrderBy(s => s.senderEmail).ToList();
                    break;
                case "sender_email_desc":
                    packages = packages.OrderByDescending(s => s.senderEmail).ToList();
                    break;
                case "sender_phone_number":
                    packages = packages.OrderBy(s => s.senderPhoneNumber).ToList();
                    break;
                case "sender_phone_number_desc":
                    packages = packages.OrderByDescending(s => s.senderPhoneNumber).ToList();
                    break;
                case "reciever_adress":
                    packages = packages.OrderBy(s => s.recieverAdress).ToList();
                    break;
                case "reciever_adress_desc":
                    packages = packages.OrderByDescending(s => s.recieverAdress).ToList();
                    break;
                case "reciever_email":
                    packages = packages.OrderBy(s => s.recieverEmail).ToList();
                    break;
                case "reciever_email_desc":
                    packages = packages.OrderByDescending(s => s.recieverEmail).ToList();
                    break;
                case "reciever_phone_number":
                    packages = packages.OrderBy(s => s.recieverPhoneNumber).ToList();
                    break;
                case "reciever_phone_number_desc":
                    packages = packages.OrderByDescending(s => s.recieverPhoneNumber).ToList();
                    break;
                case "destination":
                    packages = packages.OrderBy(s => s.destination).ToList();
                    break;
                case "destination_desc":
                    packages = packages.OrderByDescending(s => s.destination).ToList();
                    break;
                case "price":
                    packages = packages.OrderBy(s => s.price).ToList();
                    break;
                case "price_desc":
                    packages = packages.OrderByDescending(s => s.price).ToList();
                    break;
                case "paid":
                    packages = packages.OrderBy(s => s.paid).ToList();
                    break;
                case "paid_desc":
                    packages = packages.OrderByDescending(s => s.paid).ToList();
                    break;
                default:
                    packages = packages.OrderBy(s => s.senderName).ToList();
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(packages.ToPagedList(pageNumber, pageSize));
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
