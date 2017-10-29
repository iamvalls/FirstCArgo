using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.ComponentModel.DataAnnotations;


namespace FirstCargoApp.Models
{
    public class VehiculeViewModel
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "VehiculeType", ResourceType = typeof(ViewResources.Resource))]
        [Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        public string VehiculeType { get; set; }

        
        
        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        [Display(Name = "SenderName", ResourceType = typeof(ViewResources.Resource))]
        public string SenderName { get; set; }

    }
}