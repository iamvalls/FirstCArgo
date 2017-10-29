using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FirstCargoApp.Helper;
using FirstCargoApp.Models;
using System.Threading;
using System.Globalization;
using System.Web.Security;
using Microsoft.AspNet.Identity;





namespace FirstCargoApp.Controllers
{
    public class HomeController : BaseController
    {


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Login([Bind(Exclude = "oldPassword,confirmPassword")]USER model)
        {
            ModelState.Remove("oldPassword");
            ModelState.Remove("confirmPassword");
            ModelState.Remove("newPassword");

            // Lets first check if the Model is valid or not
            if (ModelState.IsValid)
            {
                using (FirstCargoDbEntities entities = new FirstCargoDbEntities ())
                {
                    string username = model.userName;
                    string password = model.password;

                    // Now if our password was enctypted or hashed we would have done the
                    // same operation on the user entered password here, But for now
                    // since the password is in plain text lets just authenticate directly

                    bool userValid = entities.USER.Any(user => user.userName == username && user.password == password);

                    // User found in the database
                    if (userValid)
                    {
                        USER user = entities.USER.SingleOrDefault(u => u.userName == username);
                        FormsAuthentication.SetAuthCookie(username + "|" + user.userID.ToString(), false);
                        int test = CurrentUserId;
                        string test2 = User.Identity.GetUserName().Split('|')[0];
                        string test3 = User.Identity.GetUserName();
                            return RedirectToAction("Vehicule", "Vehicule");                  
                        
                        //if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        //    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        //{
                        //    return Redirect(returnUrl);
                        //}
                        //else
                        //{
                        //    return RedirectToAction("Login", "Home");
                        //}
                    }
                    else
                    {
                        ModelState.AddModelError("", @ViewResources.Resource.LoginError);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);

        }


        protected int CurrentUserId
        {
            get
            {
                int userId = 0;

                if (Request.IsAuthenticated)
                {
                    userId = Convert.ToInt32(HttpContext.User.Identity.Name.Split('|')[1]);
                }

                return userId;
            }
        }

        protected string CurrentUserName
        {
            get
            {
                string userName = string.Empty;

                if (Request.IsAuthenticated)
                {
                    userName = HttpContext.User.Identity.Name.Split('|')[0];
                }

                return userName;
            }
        }

        protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string roles= string.Empty;

                        using (FirstCargoDbEntities entities = new FirstCargoDbEntities())
                        {
                            USER user = entities.USER.SingleOrDefault(u => u.userName == username);

                            if (user.isAdmin)
                            roles = "Admin";
                        }
                        //let us extract the roles from our own custom cookie


                        //Let us set the Pricipal with our user specific details
                        e.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(';'));
                    }
                    catch (Exception)
                    {
                        //somehting went wrong
                    }
                }
            }
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string roles = string.Empty;

                        using (FirstCargoDbEntities entities = new FirstCargoDbEntities())
                        {
                            USER user = entities.USER.SingleOrDefault(u => u.userName == username);

                            if (user.isAdmin)
                                roles = "Admin";
                        }
                        //let us extract the roles from our own custom cookie


                        //Let us set the Pricipal with our user specific details
                        HttpContext.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(';'));
                    }
                    catch (Exception)
                    {
                        //somehting went wrong
                    }
                }
            }
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Home");
        }

        public ActionResult Vehicule()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Package()
        {
            ViewBag.Message = "Your package page.";

            return View();
        }

        public ActionResult Other()
        {
            ViewBag.Message = "Your Other page.";

            return View();
        }

        // changing culture
        public ActionResult ChangeCulture(string ddlCulture)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ddlCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(ddlCulture);

            Session["CurrentCulture"] = ddlCulture;
            return View("Login");
        }
    }
}