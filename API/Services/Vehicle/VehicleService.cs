using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Application.DTO;
using Application.DTO.LocationDTO;
using Application.Repositories.Vehicle;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace API.Services.Vehicle
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IConfiguration _config;

        public VehicleService(IVehicleRepository vehicleRepository, IConfiguration config)
        {
            _vehicleRepository = vehicleRepository;
            _config = config;
        }

        public async Task<VehiclePositionDTO> GetCurrentPosition(string userID, string deviceID)
        {
            VehiclePositionDTO vehiclePosition = null;
            try
            {
                bool checkVehicleDevice = await _vehicleRepository.CheckVehicleDeviceviaUser(userID, deviceID).ConfigureAwait(false);
                if (checkVehicleDevice)
                {
                    vehiclePosition = await _vehicleRepository.GetCurrentPosition(userID, deviceID).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return vehiclePosition;
        }
        public async Task<LocalDTO> GetLocation(string latitude, string longitude)
        {
            LocalDTO longName = new();
            var location = $"{latitude},{longitude}";
            var YOUR_API_KEY = _config["GMap:APIKey"];
            const string BaseUrl = "https://maps.googleapis.com/maps/api/geocode";
            try
            {
                HttpClient client = new();
                client.BaseAddress = new Uri(BaseUrl);
                var responseMessage = client.GetAsync($"{BaseUrl}/json?latlng={location}&key={YOUR_API_KEY}").Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var result = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(result);
                    longName.LocalName = myDeserializedClass.results[4].formatted_address;
                }
                return longName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<List<VehiclePositionDTO>> GetVehiclePositionByDate(string userID, string deviceID, DateTime startDate, DateTime endDate)
        {
            List<VehiclePositionDTO> vehiclePosition = null;
            try
            {
                bool checkVehicleDevice = await _vehicleRepository.CheckVehicleDeviceviaUser(userID, deviceID).ConfigureAwait(false);
                if (checkVehicleDevice)
                {
                    vehiclePosition = await _vehicleRepository.GetVehiclePositionByDate(userID, deviceID, startDate, endDate).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return vehiclePosition;
        }
        public async Task<bool> RecordVehiclePosition(VehiclePositionDTO vehiclePosition)
        {
            bool checkVehicleDevice = await _vehicleRepository.CheckVehicleDeviceviaUser(vehiclePosition.UserID, vehiclePosition.DeviceID).ConfigureAwait(false);
            if (checkVehicleDevice)
            {
                return await _vehicleRepository.RecordVehiclePosition(vehiclePosition).ConfigureAwait(false);
            }
            return false;
        }
        public async Task<bool> RegisterVehicle(VehicleDTO vehicle)
        {
            int vehicleExist = await _vehicleRepository.CheckVehicleExist(vehicle.VehicleNumberPlate).ConfigureAwait(false);
            if (vehicleExist == 0)
            {
                return await _vehicleRepository.RegisterVehicle(vehicle).ConfigureAwait(false);
            }
            return false;
        }
    }
}