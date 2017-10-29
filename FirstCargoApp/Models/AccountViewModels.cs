using System;
using System.ComponentModel.DataAnnotations;

namespace FirstCargoApp.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "UserName", ResourceType = typeof(ViewResources.Resource))]  
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Aktuelles Kennwort")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "\"{0}\" muss mindestens {2} Zeichen lang sein.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Neues Kennwort")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Neues Kennwort bestätigen")]
        [Compare("NewPassword", ErrorMessage = "Das neue Kennwort stimmt nicht mit dem Bestätigungskennwort überein.")]
        public string ConfirmPassword { get; set; }
    }

    //public class LoginViewModel
    //{
    //    [Required]
    //    [Display(Name = "UserName", ResourceType = typeof(LocalResource.Resource))] 
    //    public string UserName { get; set; }

    //    [Required]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Password", ResourceType = typeof(LocalResource.Resource))] 
    //    public string Password { get; set; }

    //    [Display(Name = "Speichern?")]
    //    public bool RememberMe { get; set; }
    //}

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Benutzername")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "\"{0}\" muss mindestens {2} Zeichen lang sein.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Kennwort")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Kennwort bestätigen")]
        [Compare("Password", ErrorMessage = "Das Kennwort stimmt nicht mit dem Bestätigungskennwort überein.")]
        public string ConfirmPassword { get; set; }
    }
}
