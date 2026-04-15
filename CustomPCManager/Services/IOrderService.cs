using CustomPCManager.Models;

namespace CustomPCManager.Services
{
    /// <summary>
    /// Интерфейс сервиса для работы с заказами
    /// </summary>
    public interface IOrderService
    {
        Task<Order?> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> CreateOrderAsync(int clientId, int userId);
        Task UpdateStatusAsync(int orderId, string status, int userId);
        Task<decimal> CalculateTotalAsync(int orderId);
        Task ShipOrderAsync(int orderId, int userId);
    }
}
