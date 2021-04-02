using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTO;

namespace API.Services.Vehicle
{
    public interface IVehicleService
    {
        Task<bool> RegisterVehicle(VehicleDTO vehicle);
        Task<bool> RecordVehiclePosition(VehiclePositionDTO vehiclePosition);
        Task<VehiclePositionDTO> GetCurrentPosition(string userID, string deviceID);
        Task<List<VehiclePositionDTO>> GetVehiclePositionByDate(string userID, string deviceID, DateTime startDate, DateTime endDate);
        Task<LocalDTO> GetLocation(string latitude, string longitude);
    }
}