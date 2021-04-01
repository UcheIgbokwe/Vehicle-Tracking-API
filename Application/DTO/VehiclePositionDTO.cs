using System;

namespace Application.DTO
{
    public class VehiclePositionDTO
    {
        public string UserID { get; set; }
        public string DeviceID { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual LocalDTO Local { get; set; }
    }
}