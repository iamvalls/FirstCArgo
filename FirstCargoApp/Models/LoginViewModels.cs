using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.ComponentModel.DataAnnotations;


namespace FirstCargoApp.Models
{
    public class LoginViewModels
    {
        
        [Display(Name = "UserName", ResourceType = typeof(ViewResources.Resource))]
        [Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        public string UserName { get; set; }

        
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        [Display(Name = "Password", ResourceType = typeof(ViewResources.Resource))]
        public string Password { get; set; }

    }
}