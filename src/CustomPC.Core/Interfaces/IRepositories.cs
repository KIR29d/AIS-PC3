namespace CustomPC.Core.Interfaces;

using CustomPC.Core.Entities;

/// <summary>
/// Интерфейс репозитория для работы с пользователями
/// </summary>
public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByLoginAsync(string login);
    Task<User> CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();
}

/// <summary>
/// Интерфейс репозитория для работы с клиентами
/// </summary>
public interface IClientRepository
{
    Task<Client?> GetByIdAsync(int id);
    Task<Client> CreateAsync(Client client);
    Task UpdateAsync(Client client);
    Task DeleteAsync(int id);
    Task<IEnumerable<Client>> GetAllAsync();
    Task<IEnumerable<Client>> SearchAsync(string searchTerm);
}

/// <summary>
/// Интерфейс репозитория для работы с заказами
/// </summary>
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);
    Task<Order> CreateAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(int id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task<IEnumerable<Order>> GetByClientIdAsync(int clientId);
}

/// <summary>
/// Интерфейс репозитория для работы со сборочными заданиями
/// </summary>
public interface IAssemblyRepository
{
    Task<AssemblyTask?> GetByIdAsync(int id);
    Task<AssemblyTask> CreateAsync(AssemblyTask assemblyTask);
    Task UpdateAsync(AssemblyTask assemblyTask);
    Task DeleteAsync(int id);
    Task<IEnumerable<AssemblyTask>> GetAllAsync();
    Task<IEnumerable<AssemblyTask>> GetByOrderIdAsync(int orderId);
    Task<IEnumerable<AssemblyTask>> GetByEmployeeIdAsync(int employeeId);
}

/// <summary>
/// Интерфейс репозитория для работы с компонентами
/// </summary>
public interface IComponentRepository
{
    Task<Component?> GetByIdAsync(int id);
    Task<Component> CreateAsync(Component component);
    Task UpdateAsync(Component component);
    Task DeleteAsync(int id);
    Task<IEnumerable<Component>> GetAllAsync();
    Task<IEnumerable<Component>> SearchAsync(string searchTerm);
}

/// <summary>
/// Интерфейс репозитория для работы со складами
/// </summary>
public interface IWarehouseRepository
{
    Task<Warehouse?> GetByIdAsync(int id);
    Task<Warehouse> CreateAsync(Warehouse warehouse);
    Task UpdateAsync(Warehouse warehouse);
    Task DeleteAsync(int id);
    Task<IEnumerable<Warehouse>> GetAllAsync();
}

/// <summary>
/// Интерфейс репозитория для работы с остатками на складе
/// </summary>
public interface IStockRepository
{
    Task<StockBalance?> GetAsync(int componentId, int warehouseId);
    Task<StockBalance> CreateAsync(StockBalance stockBalance);
    Task UpdateAsync(StockBalance stockBalance);
    Task<IEnumerable<StockBalance>> GetByComponentIdAsync(int componentId);
    Task<IEnumerable<StockBalance>> GetByWarehouseIdAsync(int warehouseId);
}

/// <summary>
/// Интерфейс репозитория для работы с сотрудниками
/// </summary>
public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee> CreateAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(int id);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<IEnumerable<Employee>> GetByDepartmentIdAsync(int departmentId);
}

/// <summary>
/// Интерфейс репозитория для работы с отделами
/// </summary>
public interface IDepartmentRepository
{
    Task<Department?> GetByIdAsync(int id);
    Task<Department> CreateAsync(Department department);
    Task UpdateAsync(Department department);
    Task DeleteAsync(int id);
    Task<IEnumerable<Department>> GetAllAsync();
}

/// <summary>
/// Интерфейс репозитория для работы с ролями
/// </summary>
public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int id);
    Task<Role?> GetByNameAsync(string name);
    Task<Role> CreateAsync(Role role);
    Task UpdateAsync(Role role);
    Task DeleteAsync(int id);
    Task<IEnumerable<Role>> GetAllAsync();
}

/// <summary>
/// Интерфейс репозитория для работы с журналом аудита
/// </summary>
public interface IAuditRepository
{
    Task<AuditLog?> GetByIdAsync(int id);
    Task<AuditLog> CreateAsync(AuditLog auditLog);
    Task<IEnumerable<AuditLog>> GetByUserIdAsync(int userId);
    Task<IEnumerable<AuditLog>> GetByTableAsync(string tableName);
    Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
}
