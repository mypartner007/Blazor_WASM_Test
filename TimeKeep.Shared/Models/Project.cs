namespace TimeKeep.Shared.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description {get; set;} = default!;
        public DateTime CreatedAt {get; set;} = default!;
        public int? AssigneeId { get; set; }
        public virtual User? Assignee {get; set;} = default!;
        public List<Track>? Tracks { get; set; } = default!;

    }
}