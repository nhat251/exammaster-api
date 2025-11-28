using Domain.Interfaces;

namespace Domain.Entities
{
    public class Collection : IEntity<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Exam> Exams { get; set; } = new List<Exam>();

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
