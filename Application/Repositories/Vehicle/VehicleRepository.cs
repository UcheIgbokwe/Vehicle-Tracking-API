using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<bool> CheckVehicleDeviceviaUser(string email, string deviceId)
        {
            bool vehicleExist = false;
            var guidDeviceId = Guid.Parse(deviceId);
            try
            {
                var userResult = await CheckUserExist(email).ConfigureAwait(false);
                if (userResult != null)
                {
                    var vehicleDeviceResult = _dbContext.VehicleDevices.FirstOrDefaultAsync(x => x.DeviceID == guidDeviceId).Result;
                    if (vehicleDeviceResult != null)
                    {
                        return vehicleExist = _dbContext.Vehicles.Where(x => x.User == userResult && x.VehicleID == vehicleDeviceResult.VehicleID).Any();
                    }
                }
                return vehicleExist;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return vehicleExist;
            }
        }
        public async Task<int> CheckVehicleExist(string vehicleNumberPlate)
        {
            return await _dbContext.Vehicles.CountAsync(x => x.VehicleNumberPlate == vehicleNumberPlate).ConfigureAwait(false);
        }
        public async Task<VehiclePositionDTO> GetCurrentPosition(string userID, string deviceId)
        {
            var guidDeviceId = Guid.Parse(deviceId);
            try
            {
                //Get userId using email
                var userResult = await CheckUserExist(userID).ConfigureAwait(false);
                if (userResult != null)
                {
                    var resultSet = await _dbContext.Vehicles
                        .Join(
                            _dbContext.VehicleDevices,
                            u => u.VehicleID,
                            x => x.VehicleID,
                            (u, x) => new { VEHICLE = u, VDEVICE = x })
                        .Join(
                            _dbContext.LocationDetails,
                            u => u.VDEVICE.DeviceID,
                            x => x.DeviceID,
                            (u, x) => new { u.VEHICLE, u.VDEVICE, LOCATIONDETAILS = x })
                        .Select(s => new
                        {
                            UserID = s.VEHICLE.User.Id,
                            s.VDEVICE.DeviceID,
                            s.LOCATIONDETAILS.Latitude,
                            s.LOCATIONDETAILS.Longitude,
                            s.LOCATIONDETAILS.UpdateTime
                        })
                        .Where(w => w.UserID == userResult.Id && w.DeviceID == guidDeviceId)
                        .OrderByDescending(o => o.UpdateTime).Take(1).FirstOrDefaultAsync().ConfigureAwait(false);

                    var result = new VehiclePositionDTO()
                    {
                        UserID = resultSet.UserID,
                        DeviceID = resultSet.DeviceID.ToString(),
                        Latitude = resultSet.Latitude,
                        Longitude = resultSet.Longitude,
                        UpdateTime = resultSet.UpdateTime
                    };
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
        public async Task<List<VehiclePositionDTO>> GetVehiclePositionByDate(string userID, string deviceId, DateTime startDate, DateTime endDate)
        {
            var guidDeviceId = Guid.Parse(deviceId);
            try
            {
                //Get userId using email
                var userResult = await CheckUserExist(userID).ConfigureAwait(false);
                if (userResult != null)
                {
                    var resultSet = await _dbContext.Vehicles
                        .Join(
                            _dbContext.VehicleDevices,
                            u => u.VehicleID,
                            x => x.VehicleID,
                            (u, x) => new { VEHICLE = u, VDEVICE = x })
                        .Join(
                            _dbContext.LocationDetails,
                            u => u.VDEVICE.DeviceID,
                            x => x.DeviceID,
                            (u, x) => new { u.VEHICLE, u.VDEVICE, LOCATIONDETAILS = x })
                        .Select(s => new
                        {
                            UserID = s.VEHICLE.User.Id,
                            s.VDEVICE.DeviceID,
                            s.LOCATIONDETAILS.Latitude,
                            s.LOCATIONDETAILS.Longitude,
                            s.LOCATIONDETAILS.UpdateTime
                        })
                        .Where(w => w.UserID == userResult.Id && w.DeviceID == guidDeviceId && w.UpdateTime >= startDate && w.UpdateTime <= endDate)
                        .OrderByDescending(o => o.UpdateTime).ToListAsync().ConfigureAwait(false);

                    return resultSet.ConvertAll(x => new VehiclePositionDTO{
                        UserID = x.UserID,
                        DeviceID = x.DeviceID.ToString(),
                        Latitude = x.Latitude,
                        Longitude = x.Longitude,
                        UpdateTime = x.UpdateTime
                    });
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
        public async Task<bool> RecordVehiclePosition(VehiclePositionDTO vehiclePosition)
        {
            var guidDeviceId = Guid.Parse(vehiclePosition.DeviceID);
            try
            {
                if (vehiclePosition != null)
                {
                    var vehiclePositionModel = new LocationDetail()
                    {
                        Latitude = vehiclePosition.Latitude,
                        Longitude = vehiclePosition.Longitude,
                        UpdateTime = DateTime.Now,
                        DeviceID = guidDeviceId
                    };
                    _unitOfWork.LocationDetailRepository.Add(vehiclePositionModel);
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