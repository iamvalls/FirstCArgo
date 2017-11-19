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
        public async Task<ActionResult> Index()
        {
            return View(await db.USER.ToListAsync());
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
