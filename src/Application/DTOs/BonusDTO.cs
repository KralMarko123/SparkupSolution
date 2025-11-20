namespace SparkUpSolution.Application.DTOs
{
    public class BonusDTO
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; } 
        public int Amount { get; set; }
    }
}
