using System.Threading.Tasks;
using Application.DTO;
using Application.Repositories.Vehicle;

namespace API.Services.Vehicle
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }
        public async Task<bool> RegisterVehicle(VehicleDTO vehicle)
        {
            // Check if Vehicle Number Plate already exist
            int vehicleExist = await _vehicleRepository.CheckVehicleExist(vehicle.VehicleNumberPlate).ConfigureAwait(false);
            if (vehicleExist == 0)
            {
                return await _vehicleRepository.RegisterVehicle(vehicle).ConfigureAwait(false);
            }
            return false;
        }
    }
}