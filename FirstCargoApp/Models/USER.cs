//------------------------------------------------------------------------------
// <auto-generated>
//     Der Code wurde von einer Vorlage generiert.
//
//     Manuelle Ã„nderungen an dieser Datei fÃ¼hren mÃ¶glicherweise zu unerwartetem Verhalten der Anwendung.
//     Manuelle Ã„nderungen an dieser Datei werden Ã¼berschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FirstCargoApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class USER
    {
        [Key]
        public int userID { get; set; }
        [Display(Name = "UserName", ResourceType = typeof(ViewResources.Resource))]
        [Required(ErrorMessageResourceName = "UserNameRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        public string userName { get; set; }
        [Display(Name = "UserFirstName", ResourceType = typeof(ViewResources.Resource))]
        public string userFirstName { get; set; }
        [Display(Name = "UserLastName", ResourceType = typeof(ViewResources.Resource))]
        public string userLastName { get; set; }
        [Display(Name = "DateOfBirth", ResourceType = typeof(ViewResources.Resource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> dateOfBirth { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        [Display(Name = "Password", ResourceType = typeof(ViewResources.Resource))]
        public string password { get; set; }
        [Display(Name = "IsAdmin", ResourceType = typeof(ViewResources.Resource))]
        public bool isAdmin { get; set; }
        [Display(Name = "CreatedDate", ResourceType = typeof(ViewResources.Resource))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> createdDate { get; set; }
        public string confirmationToken { get; set; }
        [Display(Name = "IsConfirmed", ResourceType = typeof(ViewResources.Resource))]
        public bool isConfirmed { get; set; }
        public Nullable<System.DateTime> lastPasstWortFailuresDates { get; set; }
        public Nullable<System.DateTime> passwordFailuresDateSinceLastSuccess { get; set; }
        [Display(Name = "PasswordChangedDates", ResourceType = typeof(ViewResources.Resource))]
        public Nullable<System.DateTime> passwordChangedDates { get; set; }
        public string passwordSalt { get; set; }
        public string passwordVerificationToken { get; set; }
        public Nullable<System.DateTime> passwordVerificationTokenExprirationDate { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "OldPasswordRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        [Display(Name = "OldPassword", ResourceType = typeof(ViewResources.Resource))]
        public string oldPassword { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "ConfirmPasswordRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        [Compare("newPassword", ErrorMessage = "Das neue Kennwort stimmt nicht mit dem BestÃ¤tigungskennwort Ã¼berein.")]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(ViewResources.Resource))]
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^.*(?=.*[!@#$%^&*\(\)_\-+=]).*$", ErrorMessageResourceName = "PasswordRules", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        public string confirmPassword { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "NewPasswordRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        [Display(Name = "NewPassword", ResourceType = typeof(ViewResources.Resource))]
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^.*(?=.*[!@#$%^&*\(\)_\-+=]).*$", ErrorMessageResourceName = "PasswordRules", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        public string newPassword { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailAddressRequired")]
        public string email { get; set; }
        public string vCode { get; set; }
    }
}
