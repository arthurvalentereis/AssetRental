using Microsoft.EntityFrameworkCore;
using AssetRental.Domain.Entities;
using AssetRental.Domain.Interfaces.Repositories;
using AssetRental.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetRental.Infrastructure.Repositories
{
    public class DriverRepository : RepositoryBase<Driver, Guid>, IDriverRepository
    {
        private DapperDataContext _contextDapper;
        public DriverRepository(
            AssetRentalDbContext context,
            DapperDataContext contextDapper
            ) : base(context)
        {
        }
    }
}
