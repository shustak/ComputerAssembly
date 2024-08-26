namespace StoreServer.DTOs
{
    public class MotherboardDTO
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Socket { get; set; }
        public string FormFactor { get; set; }
        public string RAMType { get; set; }
        public decimal Price { get; set; }
    }
}
