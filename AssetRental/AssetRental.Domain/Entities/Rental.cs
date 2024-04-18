using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetRental.Domain.Entities
{
    public class Rental : EntityBase
    {
        public int Identifier { get; set; }
        public Motorcycle Motorcycle { get; set; }
        public Driver Driver { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public decimal TotalCost { get; set; }
    }
}
