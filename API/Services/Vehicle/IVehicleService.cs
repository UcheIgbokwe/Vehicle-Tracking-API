using System.Threading.Tasks;
using Application.DTO;

namespace API.Services.Vehicle
{
    public interface IVehicleService
    {
        Task<bool> RegisterVehicle(VehicleDTO vehicle);
        Task<bool> RecordVehiclePosition(VehiclePositionDTO vehiclePosition);
    }
}