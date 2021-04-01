using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Vehicle
    {
        [Key]
        [Column(Order = 1)]
        public Guid VehicleID { get; set; }

        public string VehicleName { get; set; }

        public string VehicleNumberPlate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdateTime { get; set; }
        public User User { get; set; }
    }
}