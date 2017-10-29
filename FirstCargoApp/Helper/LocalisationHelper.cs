using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Threading;
using System.Globalization;

namespace FirstCargoApp.Helper
{
    public class LocalisationHelper
    {

        protected HttpSessionState session;  
  
        //constructor   
        public LocalisationHelper(HttpSessionState httpSessionState)  
        {  
            session = httpSessionState;  
        }  
        // Properties  
        public static int CurrentCulture  
        {  
            get  
            {  
                if (Thread.CurrentThread.CurrentUICulture.Name == "en-GB")  
                {  
                    return 0;  
                }  
                else if (Thread.CurrentThread.CurrentUICulture.Name == "de-DE")  
                {  
                    return 1;  
                }  
                else  
                {  
                    return 0;  
                }  
            }  
            set  
            {  
  
                if (value == 0)  
                {  
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");  
                }  
                else if (value == 1)  
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");  
                }   
                else  
                {  
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;  
                }  
  
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;  
  
            }  
        }  
    }
}