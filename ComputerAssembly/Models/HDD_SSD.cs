using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerAssembly.Models
{
    public class HDD_SSD : ComponentBase
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public string Type { get; set; }

        public int Capacity { get; set; }

        public string Interface { get; set; }

        public decimal Price { get; set; }

        public HDD_SSD()
        {
        }
    }
}
