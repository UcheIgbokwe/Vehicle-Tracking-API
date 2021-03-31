using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Vehicle
    {
        [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1)]
        public Guid VehicleID { get; set; }

        [Required(ErrorMessage = "Please Vehicle Name"), MaxLength(30)]
        public string VehicleName { get; set; }

        [Required(ErrorMessage = "Please Number Plate"), MaxLength(30)]
        public string VehicleNumberPlate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdateTime { get; set; }
        public User User { get; set; }
    }
}