namespace StoreServer.DTOs
{
    public class ProcessorDTO
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Socket { get; set; }
        public double Frequency { get; set; }
        public int Cores { get; set; }
        public decimal Price { get; set; }
    }
}
