using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetRental.Domain.Entities
{
    public class Driver : EntityBase
    {
        public int Identifier { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public DateTime BirthDate { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseType { get; set; }
        public byte[] LicenseImage { get; set; }
    }
}
