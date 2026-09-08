namespace ComputerComponents.Application.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalComponents { get; set; }
        public int TotalCategories { get; set; }
        public int TotalUsers { get; set; }
        public int TotalSpecifications { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public List<ComponentDto> RecentComponents { get; set; } = new();
        public List<CategorySummaryDto> CategoriesSummary { get; set; } = new();
    }

    public class CategorySummaryDto
    {
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
