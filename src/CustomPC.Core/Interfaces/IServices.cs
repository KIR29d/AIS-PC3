namespace CustomPC.Core.Interfaces;

using CustomPC.Core.Entities;

/// <summary>
/// Интерфейс сервиса управления пользователями
/// </summary>
public interface IUserService
{
    Task<User?> RegisterAsync(string login, string password, int roleId);
    Task<User?> AuthenticateAsync(string login, string password);
    Task<User?> GetByIdAsync(int id);
    Task UpdateLastLoginAsync(int userId);
    Task<bool> HasRoleAsync(int userId, string roleName);
}

/// <summary>
/// Интерфейс сервиса управления заказами
/// </summary>
public interface IOrderService
{
    Task<Order> CreateOrderAsync(int clientId);
    Task<Order?> GetOrderByIdAsync(int id);
    Task UpdateStatusAsync(int orderId, string status);
    Task<decimal> CalculateTotalAsync(int orderId);
    Task<IEnumerable<Order>> GetOrdersByClientIdAsync(int clientId);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
}

/// <summary>
/// Интерфейс сервиса управления сборкой
/// </summary>
public interface IAssemblyService
{
    Task<AssemblyTask> CreateAssemblyTaskAsync(string name, int? orderId = null);
    Task AssignEmployeeAsync(int assemblyId, int employeeId);
    Task StartAssemblyAsync(int assemblyId, int employeeId);
    Task CompleteAssemblyAsync(int assemblyId);
    Task AddComponentAsync(int assemblyId, int componentId, int quantity);
    Task<AssemblyTask?> GetAssemblyByIdAsync(int id);
    Task<IEnumerable<AssemblyTask>> GetAssembliesByOrderIdAsync(int orderId);
}

/// <summary>
/// Интерфейс сервиса управления складом
/// </summary>
public interface IStockService
{
    Task<int> GetStockQuantityAsync(int componentId, int warehouseId);
    Task UpdateStockAsync(int componentId, int warehouseId, int quantity);
    Task<bool> ReserveComponentsAsync(int assemblyId);
    Task<bool> HasSufficientStockAsync(int componentId, int warehouseId, int quantity);
    Task<IEnumerable<StockBalance>> GetComponentStockAsync(int componentId);
}

/// <summary>
/// Интерфейс сервиса управления клиентами
/// </summary>
public interface IClientService
{
    Task<Client> CreateClientAsync(Client client);
    Task<Client?> GetClientByIdAsync(int id);
    Task UpdateClientAsync(Client client);
    Task<IEnumerable<Client>> SearchClientsAsync(string searchTerm);
}

/// <summary>
/// Интерфейс сервиса аудита
/// </summary>
public interface IAuditService
{
    Task LogInsertAsync(string tableName, int userId, string details);
    Task LogUpdateAsync(string tableName, int userId, string details);
    Task LogDeleteAsync(string tableName, int userId, string details);
    Task<IEnumerable<AuditLog>> GetAuditLogsAsync(int userId, DateTime? startDate = null, DateTime? endDate = null);
}
