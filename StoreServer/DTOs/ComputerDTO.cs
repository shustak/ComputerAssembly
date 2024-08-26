using StoreServer.DTOs;
using System.Collections.Generic;

namespace StoreServer.DTOs
{
    public class ComputerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProcessorDTO Processor { get; set; }
        public RAMDTO RAM { get; set; }
        public MotherboardDTO Motherboard { get; set; }
        public FrameDTO Frame { get; set; }
        public PowerUnitDTO PowerUnit { get; set; }
        public HDD_SSDDTO HDD_SSD { get; set; }
        public List<OtherComponentDTO> OtherComponents { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
