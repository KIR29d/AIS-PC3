using CustomPCManager.Models;

namespace CustomPCManager.Services
{
    /// <summary>
    /// Интерфейс сервиса для работы со складом
    /// </summary>
    public interface IStockService
    {
        Task<int> GetStockAsync(int componentId, int warehouseId);
        Task UpdateStockAsync(int componentId, int warehouseId, int quantity, int userId);
        Task<bool> ReserveComponentsAsync(int assemblyId, int userId);
        Task<IEnumerable<StockBalance>> GetComponentStockAsync(int componentId);
        Task<IEnumerable<StockBalance>> GetWarehouseStockAsync(int warehouseId);
    }
}
