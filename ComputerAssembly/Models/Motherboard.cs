﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerAssembly.Models
{
    public class Motherboard : ComponentBase
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public string Socket { get; set; }

        public string FormFactor { get; set; }

        public string RAMType { get; set; }

        public decimal Price { get; set; }

        public Motherboard()
        {
        }
    }
}