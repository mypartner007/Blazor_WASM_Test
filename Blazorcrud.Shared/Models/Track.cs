namespace Blazorcrud.Shared.Models
{
    public class Track
    {
        public int Id { get; set; }
        public int Hours {get; set;} = default!;
        public DateTime Date { get; set;} = default!;
        public string Receipt { get; set; } = default!;
    }
}