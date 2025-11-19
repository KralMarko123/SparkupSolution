using SparkUpSolution.Domain.Enums;

namespace SparkUpSolution.Application.Requests
{
    public class UpdateBonusRequest
    {
        public BonusStatus Status { get; set; }
        public int Amount { get; set; }
    }
}
