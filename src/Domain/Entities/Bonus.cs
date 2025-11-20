using SparkUpSolution.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SparkUpSolution.Domain.Entities
{
    public class Bonus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Player Player { get; set; } = default!;
        public BonusType Type { get; set; }
        public BonusStatus Status { get; set; }
        public int Amount { get; set; }
        public bool IsActive => Status == BonusStatus.Active;
    }
}
