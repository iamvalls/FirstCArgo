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
        public ActionResult ManageUser(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Ihr Kennwort wurde geändert."
                : message == ManageMessageId.SetPasswordSuccess ? "Ihr Kennwort wurde festgelegt."
                : message == ManageMessageId.RemoveLoginSuccess ? "Die externe Anmeldung wurde entfernt."
                : message == ManageMessageId.Error ? "Fehler"
                : "";
            
            ViewBag.ReturnUrl = Url.Action("ManageUser");
            return View();
        }

        //
        // POST: /ManageUser/ManageUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageUser(USER user)
        {
            //Todo
            //bool hasPassword = HasPassword(); // le mot de passe na#est pas encore crypeter 
            //ViewBag.HasLocalPassword = hasPassword;
            bool hasPassword = true;
            ViewBag.ReturnUrl = Url.Action("ManageUser");
            if (hasPassword)
            {
                ModelState.Remove("password");
                ModelState.Remove("userName");
                ModelState.Remove("userID");
                ModelState.Remove("email");

                int userId = user.userID;

    //            var errors3 = ModelState
    //.Where(x => x.Value.Errors.Count > 0)
    //.Select(x => new { x.Key, x.Value.Errors })
    //.ToArray();


                

                //ModelState.Remove("newPassword");
                if (ModelState.IsValid)
                {
                    
                    user.password = user.newPassword;
                    user.userName = User.Identity.Name;
                    if (user.email != null) // Just for test
                        user.email = user.email;
                    else
                        user.email = "elievalls@yahoo.fr";
                        
                    user.userID = Int32.Parse(User.Identity.GetUserName().Split('|')[1]);
                    db.Entry(user).State = EntityState.Modified;
                    try
                    {
                        await db.SaveChangesAsync();
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                    {
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
                    }
                    

                    
                    return RedirectToAction("ManageUser", new { Message = ManageMessageId.ChangePasswordSuccess });
            
                }
            }
           // Wurde dieser Punkt erreicht, ist ein Fehler aufgetreten; Formular erneut anzeigen.
            return View(user);
        }
























        // GET: /ManageUser/
        public async Task<ActionResult> Index()
        {
            return View(await db.USER.ToListAsync());
        }

        // GET: /ManageUser/Details/5
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
            ModelState.Remove("oldPassword");
            ModelState.Remove("confirmPassword");
            ModelState.Remove("newPassword");
                        var errors3 = ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .Select(x => new { x.Key, x.Value.Errors })
            .ToArray();
            if (ModelState.IsValid)
            {
                //because they donot appear to the view we need to initiliaze them 
                user.oldPassword = user.password;
                user.newPassword = user.password;
                user.confirmPassword = user.password;
                user.createdDate = DateTime.Now;
                user.confirmationToken = "";
                user.isConfirmed = false;
                db.USER.Add(user);
                try{
                await db.SaveChangesAsync();
            }catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                    {
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
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /ManageUser/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="userID,userName,userFirstName,userLastName,dateOfBirth,password,isAdmin,createdDate,confirmationToken,isConfirmed,lastPasstWortFailuresDates,passwordFailuresDateSinceLastSuccess,passwordChangedDates,passwordSalt,passwordVerificationToken,passwordVerificationTokenExprirationDate")] USER user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
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
            await db.SaveChangesAsync();
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
