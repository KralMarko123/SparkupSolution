namespace SparkUpSolution.Infrastructure.Logging
{
    public class BonusAuditLog
    {
        public Guid Id { get; set; }
        public Guid BonusId { get; set; }
        public string Action { get; set; } 
        public string PerformedById { get; set; }
        public string PerformedByName { get; set; }
        public DateTime PerformedAt { get; set; }
    }
}
