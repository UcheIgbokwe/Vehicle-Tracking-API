using System;
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
        /* Json body
         * {
	            "User": 
    	            { 
    		            "Id" : "622e664e-7101-4e47-a000-78b64b40965f"
    	            },
                "VehicleName": "KIA",
                "VehicleNumberPlate": "KJA2021NGA",
                "VehicleDevice" : 
                { 
                    "DeviceName": "Tenex" 
                }
            }
         */
        // POST: api/Vehicles
        [AllowAnonymous]
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
    }
}