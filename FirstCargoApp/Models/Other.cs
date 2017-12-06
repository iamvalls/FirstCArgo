//------------------------------------------------------------------------------
// <auto-generated>
//     Der Code wurde von einer Vorlage generiert.
//
//     Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten der Anwendung.
//     Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FirstCargoApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Other
    {
        public int otherID { get; set; }
        [Display(Name = "OtherType", ResourceType = typeof(ViewResources.Resource))]
        [Required(ErrorMessageResourceName = "OtherTypeRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        public string otherType { get; set; }
        [Display(Name = "SenderName", ResourceType = typeof(ViewResources.Resource))]
        [Required(ErrorMessageResourceName = "SenderNameRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        public string senderName { get; set; }
        [Display(Name = "SenderAdress", ResourceType = typeof(ViewResources.Resource))]
        [Required(ErrorMessageResourceName = "SenderAdressRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        public string senderAdress { get; set; }
        [Display(Name = "SenderPhoneNumber", ResourceType = typeof(ViewResources.Resource))]
        [Required(ErrorMessageResourceName = "SenderPhoneNumberRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        [RegularExpression("([0-9][0-9]*)", ErrorMessageResourceName = "DigitRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        public string senderPhoneNumber { get; set; }
        [Display(Name = "RecieverName", ResourceType = typeof(ViewResources.Resource))]
        [Required(ErrorMessageResourceName = "RecieverNameRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        public string recieverName { get; set; }
        [Display(Name = "RecieverAdress", ResourceType = typeof(ViewResources.Resource))]
        public string recieverAdress { get; set; }
        [Display(Name = "RecieverPhoneNumber", ResourceType = typeof(ViewResources.Resource))]
        [RegularExpression("([0-9][0-9]*)", ErrorMessageResourceName = "DigitRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        public string recieverPhoneNumber { get; set; }
        [Display(Name = "Destination", ResourceType = typeof(ViewResources.Resource))]
        [Required(ErrorMessageResourceName = "DestinationRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        public string destination { get; set; }
        [Display(Name = "Price", ResourceType = typeof(ViewResources.Resource))]
        [DataType("decimal(16 ,3")]
        [DisplayFormat(DataFormatString = "{0:n0}")]
        public Nullable<decimal> price { get; set; }
        [Display(Name = "Paid", ResourceType = typeof(ViewResources.Resource))]
        public bool paid { get; set; }
        [Display(Name = "Weight", ResourceType = typeof(ViewResources.Resource))]
        [Required(ErrorMessageResourceName = "WeightRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        public Nullable<decimal> weight { get; set; }
        [Display(Name = "Height", ResourceType = typeof(ViewResources.Resource))]
        [Required(ErrorMessageResourceName = "HeightRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        public Nullable<decimal> height { get; set; }
        [Display(Name = "Length", ResourceType = typeof(ViewResources.Resource))]
        [Required(ErrorMessageResourceName = "LengthRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        public Nullable<decimal> length { get; set; }
        [Display(Name = "Depth", ResourceType = typeof(ViewResources.Resource))]
        [Required(ErrorMessageResourceName = "DepthRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        public Nullable<decimal> depth { get; set; }
        [Display(Name = "ContentDescription", ResourceType = typeof(ViewResources.Resource))]
        [DataType(DataType.MultilineText)]
        public string contentDescription { get; set; }
        public Nullable<int> userID { get; set; }
        [Display(Name = "SenderEmail", ResourceType = typeof(ViewResources.Resource))]
        [EmailAddress(ErrorMessage = "EmailAddressRequired")]
        [Required(ErrorMessageResourceName = "SenderEmailRequired", ErrorMessageResourceType = typeof(ViewResources.Resource))]
        public string senderEmail { get; set; }
        [Display(Name = "RecieverEmail", ResourceType = typeof(ViewResources.Resource))]
        [EmailAddress(ErrorMessage = "EmailAddressRequired")]
        public string recieverEmail { get; set; }
  
    }
}
