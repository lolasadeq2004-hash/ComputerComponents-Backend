namespace ComputerComponents.Domain.Entities
{
    public class Specification
    {
        public int Id { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string PropertyValue { get; set; } = string.Empty;

        public int ComponentId { get; set; }
        public Component? Component { get; set; }
    }
}
