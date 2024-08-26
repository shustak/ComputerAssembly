using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreServer.Models
{
    public abstract class ComponentBase
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
    }
}
