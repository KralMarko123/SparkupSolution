using SparkUpSolution.Domain.Enums;

namespace SparkUpSolution.Application.Requests
{
    public class CreateBonusRequest
    {
        public Guid PlayerId { get; set; }
        public BonusType Type { get; set; }
        public int Amount { get; set; }
    }
}
