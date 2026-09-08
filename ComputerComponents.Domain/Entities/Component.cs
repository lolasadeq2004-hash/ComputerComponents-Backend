namespace ComputerComponents.Domain.Entities
{
    public class Component
    {
        public int Id { get; set; }
        public string ComponentName { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Specification>? Specifications { get; set; }
    }
}
