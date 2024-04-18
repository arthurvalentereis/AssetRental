using AssetRental.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetRental.Domain.Interfaces.Repositories
{
    public interface IRentalRepository : IRepositoryBase<Rental, Guid>
    {
    }
}
