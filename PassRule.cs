namespace StudentPerformanceAPI.Models
{
    public class PassRule
    {
        public int PassRuleId { get; set; }
        public decimal ThresholdPercent { get; set; }
        public decimal OverallThresholdPercent { get; set; }
    }
}
