using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1)]
        public int VehicleID { get; set; }

        [Required(ErrorMessage = "Please Vehicle Brand"), MaxLength(30)]
        public string VehicleBrand { get; set; }

        [Required(ErrorMessage = "Please Number Plate"), MaxLength(30)]
        public string VehicleNumberPlate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdateTime { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<VehicleDevice> VehicleDevices { get; set; }
    }
}