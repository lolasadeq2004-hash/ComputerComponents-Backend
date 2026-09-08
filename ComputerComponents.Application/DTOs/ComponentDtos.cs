namespace ComputerComponents.Application.DTOs
{
    public class ComponentDto
    {
        public int Id { get; set; }
        public string ComponentName { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public List<SpecificationDto>? Specifications { get; set; }
    }

    public class CreateComponentDto
    {
        public string ComponentName { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public List<CreateSpecificationDto>? Specifications { get; set; }
    }

    public class UpdateComponentDto
    {
        public int Id { get; set; }
        public string ComponentName { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public List<CreateSpecificationDto>? Specifications { get; set; }
    }
}
