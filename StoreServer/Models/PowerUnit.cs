using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreServer.Models
{
    public class PowerUnit : ComponentBase
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public int Wattage { get; set; }

        
        public string FormFactor { get; set; }

        public decimal Price { get; set; }
        public PowerUnit()
        {
        }
    }
}
