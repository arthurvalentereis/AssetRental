using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetRental.Domain.Entities
{
    public class RentalPlan : EntityBase
    {
        public int Days { get; set; }
        public decimal DailyCost { get; set; }
    }
}