using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreServer.Models
{
    public class Processor : ComponentBase
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public string Socket { get; set; }

        public double Frequency { get; set; }

        public int Cores { get; set; }

        public decimal Price { get; set; }

        public Processor()
        {
        }
    }

}
