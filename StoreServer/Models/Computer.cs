using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;

namespace StoreServer.Models
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
                decimal price = Processor.Price + RAM.Price + Motherboard.Price + HDD_SSD.Price + PowerUnit.Price + Frame.Price;
                foreach (var component in OtherComponents)
                {
                    price += component.Price;
                }
                return price;
            }
        }

        public Computer()
        {
            OtherComponents = new List<OtherComponent>();
        }
    }
}