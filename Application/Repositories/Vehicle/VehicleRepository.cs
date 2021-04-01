using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.DTO;
using Application.UnitOfWork;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Repositories.Vehicle
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DataContext _dbContext;
        private readonly ILogger<VehicleRepository> _logger;
        public VehicleRepository(IUnitOfWork unitOfWork, DataContext dbContext, ILogger<VehicleRepository> logger)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<User> CheckUserExist(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email).ConfigureAwait(false);
        }
        public async Task<int> CheckVehicleExist(string vehicleNumberPlate)
        {
            return await _dbContext.Vehicles.CountAsync(x => x.VehicleNumberPlate == vehicleNumberPlate).ConfigureAwait(false);
        }
        public Task<VehiclePositionDTO> GetCurrentVehiclePosition(string userID, string deviceId)
        {
            throw new NotImplementedException();
        }

        public Task<List<VehiclePositionDTO>> GetVehiclePosition(string userID, string deviceId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RecordVehiclePosition(VehiclePositionDTO vehiclePosition)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RegisterVehicle(VehicleDTO vehicle)
        {
            try
            {
                var userResult = await CheckUserExist(vehicle.User.Id).ConfigureAwait(false);
                if (vehicle != null)
                {
                    var newVehicle = new Domain.Models.Vehicle()
                    {
                        VehicleName = vehicle.VehicleName,
                        VehicleNumberPlate = vehicle.VehicleNumberPlate,
                        UpdateTime = DateTime.Now,
                        User = userResult
                    };
                    _unitOfWork.VehicleRepository.Add(newVehicle);

                    var newDevice = new Domain.Models.VehicleDevice()
                    {
                        VehicleID = newVehicle.VehicleID,
                        DeviceName = vehicle.VehicleDevice.DeviceName
                    };
                    _unitOfWork.VehicleDeviceRepository.Add(newDevice);

                    var result = await _unitOfWork.CommitAsync().ConfigureAwait(false);
                    if (result > 0)
                        return true;
                }
                return false;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}