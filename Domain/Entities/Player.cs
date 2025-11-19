namespace SparkUpSolution.Domain.Entities
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public ICollection<Bonus> Bonuses { get; set; } = new List<Bonus>();
    }
}
