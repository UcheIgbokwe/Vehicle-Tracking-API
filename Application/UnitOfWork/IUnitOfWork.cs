using System;
using System.Threading.Tasks;
using Application.Infrastructure;
using Domain.Models;

namespace Application.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync();
        IRepository<Vehicle> VehicleRepository { get; }
        IRepository<VehicleDevice> VehicleDeviceRepository { get; }
        IRepository<LocationDetail> LocationDetailRepository { get; }
    }
}