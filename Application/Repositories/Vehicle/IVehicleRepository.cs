using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTO;
using Domain.Models;

namespace Application.Repositories.Vehicle
{
    public interface IVehicleRepository
    {
        Task<bool> RegisterVehicle(VehicleDTO vehicle);
        Task<bool> RecordVehiclePosition(VehiclePositionDTO vehiclePosition);
        Task<VehiclePositionDTO> GetCurrentPosition(string userID, string deviceId);
        Task<List<VehiclePositionDTO>> GetVehiclePositionByDate(string userID, string deviceId, DateTime startDate, DateTime endDate);

        Task<int> CheckVehicleExist(string vehicleNumberPlate);
        Task<User> CheckUserExist(string emailId);
        Task<bool> CheckVehicleDeviceviaUser(string userID, string deviceId);
    }
}