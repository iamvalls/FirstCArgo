using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Web.Mvc;
using FirstCargoApp.Models;
using FirstCargoApp.Helper;
using System.Threading;
using System.Globalization;
using PagedList;

namespace FirstCargoApp.Controllers
{
    public class ManageUserController : Controller
    {
        private FirstCargoDbEntities db = new FirstCargoDbEntities();

        public ManageUserController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public ManageUserController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Home/ManageUser
        [HttpGet]
        public ActionResult ManageUser(NotificationMessage.ManageMessageId? message)
        {

            ViewBag.StatusMessage =
                message == NotificationMessage.ManageMessageId.ChangePasswordSuccess ? @ViewResources.Resource.ChangePasswordSuccess
                : message == NotificationMessage.ManageMessageId.Error ? @ViewResources.Resource.Error
                : message == NotificationMessage.ManageMessageId.NoEntryFound ? @ViewResources.Resource.NoEntryFound
                : "";
            
            ViewBag.ReturnUrl = Url.Action("ManageUser");

            
            return View();
        }

        // This method change the Password of a Logged User
        // POST: /ManageUser/ManageUser 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageUser(USER user)
        {

            ViewBag.ReturnUrl = Url.Action("ManageUser");

                // Remove the useles data column because we dont#t need them to Change the password
                ModelState.Remove("password");
                ModelState.Remove("userName");
                ModelState.Remove("userID");
                ModelState.Remove("email");

                int userId = user.userID;

    //            var errors3 = ModelState
    //.Where(x => x.Value.Errors.Count > 0)
    //.Select(x => new { x.Key, x.Value.Errors })
    //.ToArray();

                if (ModelState.IsValid)
                {
                    // Set obligated User Property before updated the changes 
                    user.password = user.newPassword;
                    user.userName = User.Identity.GetUserName().Split('|')[0].ToString();
                    user.userID = Int32.Parse(User.Identity.GetUserName().Split('|')[1]);
                    using (FirstCargoDbEntities entities = new FirstCargoDbEntities())
                    {
                        USER userToUpdate = entities.USER.SingleOrDefault(u => u.userName == user.userName);
                        var hashCode = userToUpdate.vCode;
                        //Password Hasing Process Call Helper Class Method    
                        var encodingPasswordString = RegistrationLoginHelper.EncodePassword(user.oldPassword, hashCode);

                        if (encodingPasswordString.Equals(userToUpdate.password))
                        {

                            //Check Login Detail User Name Or Password    
                            var query = (from s in entities.USER where (s.userName == user.userName || s.email == user.userName) && s.password.Equals(encodingPasswordString) select s).FirstOrDefault();

                            if(query != null){

        
                                var password = RegistrationLoginHelper.EncodePassword(user.newPassword, hashCode);
                                userToUpdate.oldPassword = userToUpdate.password;
                                userToUpdate.password = userToUpdate.newPassword = userToUpdate.confirmPassword =password;
                                userToUpdate.passwordChangedDates = DateTime.Now;

                                db.Entry(userToUpdate).State = EntityState.Modified;
                                try
                                {
                                    await db.SaveChangesAsync();
                                }
                                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                                {
                                    // Todo Log the error
                                }
                            }

                        }
                    }
                    return RedirectToAction("ManageUser", new { Message = NotificationMessage.ManageMessageId.ChangePasswordSuccess });
                }
           // how form again if there is a failure
            return View(user);
        }

        // GET: /ManageUser/ Show the list of User
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            // Get all entries from DB
            var users = await db.USER.ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.userFirstName.Contains(searchString)
                                       || s.userLastName.Contains(searchString)).ToList();
            }

            // In case there is no entry
            if (users.Count == 0)
                return RedirectToAction("Index", new { Message = NotificationMessage.ManageMessageId.NoEntryFound });

            // Viewbag for Sort

            ViewBag.UserNameSortParm = String.IsNullOrEmpty(sortOrder) ? "user_name_desc" : "";
            ViewBag.UserFirstNameSortParm = sortOrder == "user_first_name" ? "user_first_name_desc" : "user_first_name";
            ViewBag.UserLastNameSortParm = sortOrder == "user_last_name" ? "user_last_name_desc" : "user_last_name";
            ViewBag.EmailSortParm = sortOrder == "email" ? "email_desc" : "email";
            ViewBag.IsAdminSortParm = sortOrder == "is_admin" ? "is_admin_desc" : "is_admin";
            ViewBag.CreatedDateSortParm = sortOrder == "created_date" ? "created_date_desc" : "created_date";
            ViewBag.IsConfirmedSortParm = sortOrder == "is_confirmed" ? "is_confirmed_desc" : "is_confirmed";

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
                case "user_name_desc":
                    users = users.OrderByDescending(s => s.userName).ToList();
                    break;
                case "user_first_name":
                    users = users.OrderBy(s => s.userFirstName).ToList();
                    break;
                case "user_first_name_desc":
                    users = users.OrderByDescending(s => s.userFirstName).ToList();
                    break;
                case "user_last_name":
                    users = users.OrderBy(s => s.userLastName).ToList();
                    break;
                case "user_last_name_desc":
                    users = users.OrderByDescending(s => s.userLastName).ToList();
                    break;
                case "email":
                    users = users.OrderBy(s => s.email).ToList();
                    break;
                case "email_desc":
                    users = users.OrderByDescending(s => s.email).ToList();
                    break;
                case "is_admin":
                    users = users.OrderBy(s => s.isAdmin).ToList();
                    break;
                case "is_admin_desc":
                    users = users.OrderByDescending(s => s.isAdmin).ToList();
                    break;
                case "is_confirmed":
                    users = users.OrderBy(s => s.isConfirmed).ToList();
                    break;
                case "is_confirmed_desc":
                    users = users.OrderByDescending(s => s.isConfirmed).ToList();
                    break;
                default:
                    users = users.OrderBy(s => s.userName).ToList();
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(users.ToPagedList(pageNumber, pageSize));
        }

        // GET: /ManageUser/ Show user Details
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER user = await db.USER.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: /ManageUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ManageUser/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="userName,userFirstName,userLastName,dateOfBirth,password,isAdmin,email")] USER user)
        {
            // Remove the useles data column because we dont#t need them to Change the password
            ModelState.Remove("oldPassword");
            ModelState.Remove("confirmPassword");
            ModelState.Remove("newPassword");

                        var errors3 = ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .Select(x => new { x.Key, x.Value.Errors })
            .ToArray(); // This has to be remove later

            if (ModelState.IsValid)
            {
                try
                {

                    var chkUser = (from s in db.USER where s.userName == user.userName || s.email == user.email select s).FirstOrDefault();
                if (chkUser == null)
                {
                    var keyNew = RegistrationLoginHelper.GeneratePassword(10);
                    var password = RegistrationLoginHelper.EncodePassword(user.password, keyNew);
                    user.password = password;
                    user.createdDate = DateTime.Now;
                    user.vCode = keyNew;
                    //because they donot appear to the view we need to initiliaze them 
                    // Set obligated User Property before updated the changes 
                    user.oldPassword = user.password;
                    user.newPassword = user.password;
                    user.confirmPassword = user.password;
                    user.createdDate = DateTime.Now;
                    user.confirmationToken = "";
                    user.isConfirmed = false;
 
                    db.USER.Add(user);
                    await db.SaveChangesAsync();
                    //return RedirectToAction("LogIn", "Login");
                }
                ViewBag.ErrorMessage = "User Allredy Exixts!!!!!!!!!!"; 
}catch (System.Data.Entity.Validation.DbEntityValidationException dbEx){
                        Exception raise = dbEx;
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                string message = string.Format("{0}:{1}",
                                    validationErrors.Entry.Entity.ToString(),
                                    validationError.ErrorMessage);
                                // raise a new exception nesting
                                // the current instance as InnerException
                                raise = new InvalidOperationException(message, raise);
                            }
                        }
                        throw raise;

                        //Todo Log the exception
                    }
                
                return RedirectToAction("Index","ManageUser");
            }

            return View(user);
        }

        // GET: /ManageUser/Edit/5 
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER user = await db.USER.FindAsync(id);

            String.Format("{0:d/M/yyyy HH:mm:ss}", user.createdDate);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /ManageUser/Edit/5 user 
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="userID,userName,userFirstName,userLastName,dateOfBirth,password,isAdmin,createdDate,confirmationToken,isConfirmed,lastPasstWortFailuresDates,passwordFailuresDateSinceLastSuccess,passwordChangedDates,passwordSalt,passwordVerificationToken,passwordVerificationTokenExprirationDate")] USER user)
        {
            // Remove the useles data column because we dont#t need them to edit the User. They dont apear on the view
            ModelState.Remove("confirmPassword");
            ModelState.Remove("newPassword");
            ModelState.Remove("passwordVerificationTokenExprirationDate");
            ModelState.Remove("passwordVerificationToken");
            ModelState.Remove("passwordSalt");
            ModelState.Remove("passwordFailuresDateSinceLastSuccess");
            ModelState.Remove("passwordFailuresDateSinceLastSuccess");
            ModelState.Remove("confirmationToken");
            ModelState.Remove("passwordFailuresDateSinceLastSuccess");
            ModelState.Remove("email");

            if (ModelState.IsValid)
            {
                //because they do not appear to the view we need to initiliaze them 
                user.oldPassword = user.password;
                user.newPassword = user.password;
                user.confirmPassword = user.password;
                user.createdDate = DateTime.Now;
                user.email = user.email;
                user.confirmationToken = "";
                user.isConfirmed = false;

                db.Entry(user).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {

                    //Todo Log exception
                }
                
                return RedirectToAction("Index", "ManageUser");
            }
            return View(user);
        }

        // GET: /ManageUser/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER user = await db.USER.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /ManageUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            USER user = await db.USER.FindAsync(id);
            db.USER.Remove(user);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {

                //Todo Log exception
            }
            
            return RedirectToAction("Index", "ManageUser");
        }

        
        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
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
