using AssetRental.Domain.Entities;
using AssetRental.Domain.Interfaces.Repositories;
using AssetRental.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetRental.Domain.Services
{
    public class DriverService : IDriverService
    {
        private readonly IUnitOfWork? _unitOfWork;
        private List<Driver> drivers;

        public DriverService()
        {
            drivers = new List<Driver>();
        }

        public Driver RegisterDriver(Driver driver)
        {

            drivers.Add(driver);
            return driver;
        }

        public void SendLicensePhoto(int identifier, byte[] licenseImage)
        {
            var driver = drivers.FirstOrDefault(d => d.Identifier == identifier);
            if (driver != null)
            {
                driver.LicenseImage = licenseImage;
            }
        }
    }
}
