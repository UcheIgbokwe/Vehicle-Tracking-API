using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class VehicleDevice
    {
        [Key]
        public Guid DeviceID { get; set; }

        public string DeviceName { get; set; }

        [ForeignKey("Vehicle")]
        public Guid VehicleID { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        public virtual ICollection<LocationDetail> LocationDetails { get; set; }
    }
}