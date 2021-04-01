using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;

namespace Application.DTO
{
    public class VehicleDTO
    {
        public string VehicleID { get; set; }

        [Required(ErrorMessage = "Please Vehicle Name")]
        public string VehicleName { get; set; }

        [Required(ErrorMessage = "Please Number Plate")]
        public string VehicleNumberPlate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdateTime { get; set; }
        public UserDto User { get; set; }
        public VehicleDeviceDTO VehicleDevice { get; set; }
        public LocationDetailDTO Location { get; set; }
    }
}