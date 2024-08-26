using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreServer.Models
{
    public class RAM : ComponentBase
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public int Size { get; set; }

        public int Frequency { get; set; }

       public string Model { get; set; } = "Unknown Model";

        public decimal Price { get; set; }

        public RAM()
        {
        }
    }
}
