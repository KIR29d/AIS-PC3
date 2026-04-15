namespace CustomPCManager.Repositories
{
    /// <summary>
    /// Интерфейс репозитория для работы с заказами
    /// </summary>
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetByClientIdAsync(int clientId);
        Task<int> CreateAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
        Task<IEnumerable<Order>> GetByStatusAsync(string status);
    }
}
