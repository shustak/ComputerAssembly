using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerAssembly.Models
{
    public class OtherComponent : ComponentBase
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string Model { get; set; }

        public string Manufacturer { get; set; }

        public decimal Price { get; set; }

        public OtherComponent()
        {
        }
    }

}
