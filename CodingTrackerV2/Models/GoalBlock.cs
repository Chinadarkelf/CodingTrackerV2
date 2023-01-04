namespace CodingTrackerV2.Models
{
    internal class GoalBlock
    {
        public int Id { get; set; }
        public int Hours { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public int Progress { get; set; }
    }
}
