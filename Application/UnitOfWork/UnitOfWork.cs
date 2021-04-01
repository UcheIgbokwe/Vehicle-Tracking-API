using System;
using System.Threading.Tasks;
using Application.Infrastructure;
using Domain.Models;
using Persistence;

namespace Application.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dbContext;

        public UnitOfWork(DataContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException("dbContext");
        }
        private IRepository<Vehicle> _VehicleRepository;
        public IRepository<Vehicle> VehicleRepository
        {
            get { return _VehicleRepository ??= new RepositoryBase<Vehicle>(_dbContext); }
        }
        public IRepository<VehicleDevice> _VehicleDeviceRepository;
        public IRepository<VehicleDevice> VehicleDeviceRepository
        {
            get { return _VehicleDeviceRepository ??= new RepositoryBase<VehicleDevice>(_dbContext); }
        }

        public IRepository<LocationDetail> _LocationDetailRepository;
        public IRepository<LocationDetail> LocationDetailRepository
        {
            get { return _LocationDetailRepository ??= new RepositoryBase<LocationDetail>(_dbContext); }
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        void IDisposable.Dispose()
        {
            if (_dbContext != null)
                _dbContext.Dispose();
        }
    }
}