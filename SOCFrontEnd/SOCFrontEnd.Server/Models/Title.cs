namespace SOCDataManager.Models
{
    public class Title
    {
        public int TitleId { get; set; }
        public string? TitleName { get; set; }
        public string? Author { get; set; }
        public string? Isbn { get; set; }
        public DateTime? PublishDate { get; set; }
        public Spot? Spot { get; set; }
    }
}
