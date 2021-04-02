using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Services.Vehicle;
using Application.DTO;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        // Register a new vehicle
        /* 
         * {
	            "User": 
    	            { 
    		            "email" : "uche@test.com"
    	            },
                "VehicleName": "KIA",
                "VehicleNumberPlate": "KJA2021NGA",
                "VehicleDevice" : 
                { 
                    "DeviceName": "Tenex" 
                }
            }
         */
        // POST: api/Vehicles/registerVehicle
        [Authorize]
        [HttpPost("registerVehicle")]
        public async Task<ActionResult> RegisterVehicle(VehicleDTO vehicle)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool result = await _vehicleService.RegisterVehicle(vehicle).ConfigureAwait(false);
                if (result)
                    return StatusCode(201);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Register a vehicle's position
        /*
        * {
            "UserID": "uche@test.com",        
            "DeviceId": "65EC0292-409D-46E8-96A2-AB2AE86E972A",
	        "Latitude": 6.412850,
            "Longitude": 4.087600,
          }
         */
        // POST: api/Vehicles/registerVehiclePosition
        [Authorize]
        [HttpPost("registerVehiclePosition")]
        public async Task<ActionResult> RegisterVehiclePosition(VehiclePositionDTO vehiclePosition)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool result = await _vehicleService.RecordVehiclePosition(vehiclePosition).ConfigureAwait(false);
                if (result)
                    return StatusCode(201);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Get a Vehicle's position.
        // GET: /api/Vehicles?userID={email}&deviceId={id}
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetCurrentPosition(string userID, string deviceId)
        {
            VehiclePositionDTO vehiclePosition = null;
            try
            {
                if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(deviceId))
                {
                    vehiclePosition = await _vehicleService.GetCurrentPosition(userID, deviceId).ConfigureAwait(false);
                    if (vehiclePosition == null)
                    {
                        return NotFound();
                    }
                    // Google Map Api for local name                     
                    vehiclePosition.Local = await _vehicleService.GetLocation(vehiclePosition.Latitude.ToString(),vehiclePosition.Longitude.ToString()).ConfigureAwait(false);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(vehiclePosition);
        }

        // Get a Vehicle's position using date range.
        // GET: /api/Vehicles/byDate?userID={email}&deviceId={id}&startDate={sd}&endDate={ed}
        [Authorize]
        [HttpGet("byDate")]
        public async Task<ActionResult> GetCurrentPositionByDate(string userID, string deviceId, DateTime startDate, DateTime endDate)
        {
            List<VehiclePositionDTO> vehiclePosition = null;
            try
            {
                if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(deviceId))
                {
                    vehiclePosition = await _vehicleService.GetVehiclePositionByDate(userID, deviceId, startDate, endDate).ConfigureAwait(false);
                    if (vehiclePosition == null)
                    {
                        return NotFound();
                    }
                    foreach (var item in vehiclePosition)
                    {
                        // Google Map Api for local name                     
                        item.Local = await _vehicleService.GetLocation(item.Latitude.ToString(),item.Longitude.ToString()).ConfigureAwait(false);
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(vehiclePosition);
        }
    }
}