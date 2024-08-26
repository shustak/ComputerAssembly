using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Windows.Controls;

namespace ComputerAssembly.Models
{
    public class Computer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProcessorId { get; set; }
        public Processor Processor { get; set; }
        public int RAMId { get; set; }
        public RAM RAM { get; set; }
        public int MotherboardId { get; set; }
        public Motherboard Motherboard { get; set; }
        public int StorageId { get; set; }
        public HDD_SSD HDD_SSD { get; set; }
        public int PowerUnitId { get; set; }
        public PowerUnit PowerUnit { get; set; }
        public int FrameId { get; set; }
        public Frame Frame { get; set; }
        public List<OtherComponent> OtherComponents { get; set; } = new List<OtherComponent>();
        public decimal Price { get; set; }

        public decimal TotalPrice
        {
            get
            {
                decimal price =
                    (Processor?.Price ?? 0) +
                    (RAM?.Price ?? 0) +
                    (Motherboard?.Price ?? 0) +
                    (HDD_SSD?.Price ?? 0) +
                    (PowerUnit?.Price ?? 0) +
                    (Frame?.Price ?? 0);

                foreach (var component in OtherComponents)
                {
                    price += component?.Price ?? 0;
                }

                return price;
            }
        }
    }
}