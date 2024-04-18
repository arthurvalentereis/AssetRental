using AssetRental.Domain.Entities;
using AssetRental.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetRental.Domain.Services
{
    public class MotorcycleService : IMotorcycleService
    {
        private List<Motorcycle> motorcycles;

        public MotorcycleService()
        {
            motorcycles = new List<Motorcycle>();
        }

        public void RegisterMotorcycle(Motorcycle motorcycle)
        {
            motorcycles.Add(motorcycle);
        }

        public List<Motorcycle> QueryMotorcycles(string licensePlate)
        {
            return motorcycles.Where(m => m.LicensePlate == licensePlate).ToList();
        }

        public void ModifyMotorcycleLicensePlate(int identifier, string newLicensePlate)
        {
            var motorcycle = motorcycles.FirstOrDefault(m => m.Identifier == identifier);
            if (motorcycle != null)
            {
                motorcycle.LicensePlate = newLicensePlate;
            }
        }

        public void RemoveMotorcycle(int identifier)
        {
            motorcycles.RemoveAll(m => m.Identifier == identifier);
        }
    }

}
