using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTO;

namespace Application.Repositories.Vehicle
{
    public interface IVehicleRepository
    {
        Task<bool> RegisterVehicle(VehicleDTO vehicle);
        Task<bool> RecordVehiclePosition(VehiclePositionDTO vehiclePosition);
        Task<VehiclePositionDTO> GetCurrentVehiclePosition(string userID, string deviceId);
        Task<List<VehiclePositionDTO>> GetVehiclePosition(string userID, string deviceId, DateTime startDate, DateTime endDate);

        Task<int> CheckVehicleExist(string vehicleNumberPlate);
        //Task<int> CheckUserExist(string emailId);
        //Task<int> CheckUserWithVehicleDevice(int userID, int deviceId);
    }
}