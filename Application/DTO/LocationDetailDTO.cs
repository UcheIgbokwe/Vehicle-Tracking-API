using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.DTO
{
    public class LocationDetailDTO
    {
        public int LocationId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdateTime { get; set; }
    }
}