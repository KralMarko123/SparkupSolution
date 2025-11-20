using System.ComponentModel.DataAnnotations;

namespace SparkUpSolution.Domain.Entities
{
    public class Player
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public ICollection<Bonus> Bonuses { get; set; } = new List<Bonus>();
    }
}
