namespace ComputerComponents.Application.DTOs
{
    public class SpecificationDto
    {
        public int Id { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string PropertyValue { get; set; } = string.Empty;
        public int ComponentId { get; set; }
        public string? ComponentName { get; set; }
    }

    public class CreateSpecificationDto
    {
        public string PropertyName { get; set; } = string.Empty;
        public string PropertyValue { get; set; } = string.Empty;
        public int ComponentId { get; set; }
    }

    public class UpdateSpecificationDto
    {
        public int Id { get; set; }
        public string PropertyName { get; set; } = string.Empty;
        public string PropertyValue { get; set; } = string.Empty;
        public int ComponentId { get; set; }
    }
}
