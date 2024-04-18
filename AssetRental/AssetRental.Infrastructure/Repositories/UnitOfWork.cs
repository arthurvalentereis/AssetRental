using AssetRental.Domain.Interfaces.Repositories;
using AssetRental.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetRental.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AssetRentalDbContext? _AssetRentalDbContext;
        private readonly DapperDataContext? _contextDapper;
        private readonly ApplicationDbContext? _identityContext;

        public UnitOfWork(AssetRentalDbContext? AssetRentalDbContext,
                          DapperDataContext? contextDapper,
                          ApplicationDbContext? identityContext)
        {
            _AssetRentalDbContext = AssetRentalDbContext;
            _contextDapper = contextDapper;
            _identityContext = identityContext;
        }


        #region AssetRentalDbContext
        public IDriverRepository DriverRepository => new DriverRepository(_AssetRentalDbContext, _contextDapper);
        public IMotorcycleRepository MotorcycleRepository => new MotorcycleRepository(_AssetRentalDbContext, _contextDapper);
        public IRentalPlanRepository RentalPlanRepository => new RentalPlanRepository(_AssetRentalDbContext, _contextDapper);
        public IRentalRepository RentalRepository => new RentalRepository(_AssetRentalDbContext, _contextDapper);

        #endregion

        #region IdentityDbContext
        //public UserRepository UserRepository => new UserRepository(_identityContext);
        //public GroupRepository GroupRepository => new GroupRepository(_identityContext);
        //public UserRolesRepository UserRolesRepository => new UserRolesRepository(_identityContext);
        #endregion



        public void Dispose()
        {
            _AssetRentalDbContext.Dispose();
            _identityContext.Dispose();
            _contextDapper.Dispose();
        }

        public void SaveChanges()
        {
            _AssetRentalDbContext.SaveChanges();
            _identityContext.SaveChanges();
        }

    }
}
