using SparkUpSolution.Domain.Enums;

namespace SparkUpSolution.Domain.Entities
{
    public class Bonus
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Player Player { get; set; } = default!;
        public BonusType Type { get; set; }
        public BonusStatus Status { get; set; }
        public int Amount { get; set; }
        public bool IsActive => Status == BonusStatus.Active;
    }
}
