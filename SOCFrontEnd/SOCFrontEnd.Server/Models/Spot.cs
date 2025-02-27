namespace SOCDataManager.Models
{
    public class Spot
    {
        public int SpotId { get; set; }
        public string? SpotName { get; set; }
        public List<Title>? Titles { get; set; }
    }
}
