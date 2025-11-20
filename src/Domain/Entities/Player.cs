using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SparkUpSolution.Domain.Entities
{
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public ICollection<Bonus> Bonuses { get; set; } = new List<Bonus>();
    }
}
