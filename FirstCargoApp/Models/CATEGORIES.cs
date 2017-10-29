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
    
    public partial class CATEGORIES
    {
        public CATEGORIES()
        {
            this.OTHERS = new HashSet<OTHERS>();
            this.VEHICULES = new HashSet<VEHICULES>();
            this.PACKAGES = new HashSet<PACKAGES>();
        }
    
        public int categoryID { get; set; }
        public string categoryName { get; set; }
        public string senderName { get; set; }
        public string senderAdress { get; set; }
        public string senderPhoneNumber { get; set; }
        public string recieverName { get; set; }
        public string recieverAdress { get; set; }
        public string recieverPhoneNumber { get; set; }
        public string destination { get; set; }
        public Nullable<decimal> price { get; set; }
        public bool paid { get; set; }
        public Nullable<decimal> weight { get; set; }
        public Nullable<decimal> height { get; set; }
        public Nullable<decimal> length { get; set; }
        public Nullable<decimal> depth { get; set; }
        public string contentDescription { get; set; }
        public string userID { get; set; }
    
        public virtual ICollection<OTHERS> OTHERS { get; set; }
        public virtual ICollection<VEHICULES> VEHICULES { get; set; }
        public virtual ICollection<PACKAGES> PACKAGES { get; set; }
    }
}
