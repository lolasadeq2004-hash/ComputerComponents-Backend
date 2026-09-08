using ComputerComponents.Application.DTOs;

namespace ComputerComponents.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetStatsAsync();
    }
}
