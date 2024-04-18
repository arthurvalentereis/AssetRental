using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetRental.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRentalPlanRepository RentalPlanRepository { get; }
        IRentalRepository RentalRepository { get; }
        IDriverRepository DriverRepository { get; }
        IMotorcycleRepository MotorcycleRepository { get; }

        void SaveChanges();
    }
}
