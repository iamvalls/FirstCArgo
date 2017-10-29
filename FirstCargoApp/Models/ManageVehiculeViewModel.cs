using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstCargoApp.Models
{
    [Table("vehiculedetails")]
    public class ManageVehiculeViewModel
    {

        [Key]
        public int id { get; set; }
        public string vehiculeName { get; set; }
        public string vehiculeType { get; set; }

    }
}