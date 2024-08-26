﻿namespace StoreServer.DTOs
{
    public class PowerUnitDTO
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public int Wattage { get; set; }
        public string FormFactor { get; set; }
        public decimal Price { get; set; }
    }
}
