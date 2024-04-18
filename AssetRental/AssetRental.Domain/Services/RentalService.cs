using AssetRental.Domain.Entities;
using AssetRental.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetRental.Domain.Services
{
    public class RentalService : IRentalService
    {
        private List<Rental> rentals;
        private List<RentalPlan> rentalPlans;

        public RentalService()
        {
            rentals = new List<Rental>();
            rentalPlans = new List<RentalPlan>
        {
            new RentalPlan { Days = 7, DailyCost = 30.00m },
            new RentalPlan { Days = 15, DailyCost = 28.00m },
            new RentalPlan { Days = 30, DailyCost = 22.00m },
            new RentalPlan { Days = 45, DailyCost = 20.00m },
            new RentalPlan { Days = 50, DailyCost = 18.00m }
        };
        }

        public void RentMotorcycle(Motorcycle motorcycle, Driver driver, DateTime startDate, RentalPlan plan)
        {
            var rental = new Rental
            {
                Identifier = rentals.Count + 1,
                Motorcycle = motorcycle,
                Driver = driver,
                StartDate = startDate,
                EndDate = startDate.AddDays(plan.Days),
                ExpectedEndDate = startDate.AddDays(plan.Days),
                TotalCost = plan.Days * plan.DailyCost
            };
            rentals.Add(rental);
        }

        public decimal CalculateTotalRentalCost(DateTime returnDate, Rental rental)
        {
            decimal totalCost = 0;

            if (returnDate < rental.ExpectedEndDate)
            {
                // Calculate cost with penalty
                int extraDays = (int)(rental.ExpectedEndDate - returnDate).TotalDays;
                decimal penalty = 0;
                if (rental.Motorcycle != null && rental.Motorcycle.Identifier == 1) // Assuming plan of 7 days
                {
                    penalty = 0.2m * extraDays * rental.Motorcycle.Year;
                }
                else if (rental.Motorcycle != null && rental.Motorcycle.Identifier == 2) // Assuming plan of 15 days
                {
                    penalty = 0.4m * extraDays * rental.Motorcycle.Year;
                }
                totalCost = rental.TotalCost + penalty;
            }
            else if (returnDate > rental.ExpectedEndDate)
            {
                // Calculate cost with additional days
                int extraDays = (int)(returnDate - rental.ExpectedEndDate).TotalDays;
                totalCost = rental.TotalCost + extraDays * 50.00m;
            }
            else
            {
                // No penalty or additional cost
                totalCost = rental.TotalCost;
            }

            return totalCost;
        }
    }
}
