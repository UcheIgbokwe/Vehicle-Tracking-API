using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class LocationDetail
    {
        [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1)]
        public Guid LocationId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdateTime { get; set; }

        [ForeignKey("VehicleDevice")]
        public Guid DeviceID { get; set; }
        public virtual VehicleDevice VehicleDevice { get; set; }
    }
}