using CustomPC.Core.Entities;
using CustomPC.Core.Interfaces;
using CustomPC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomPC.Infrastructure.Repositories;

/// <summary>
/// Репозиторий для работы с пользователями
/// </summary>
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(CustomPcDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByLoginAsync(string login)
    {
        return await _dbSet
            .Include(u => u.роль)
            .FirstOrDefaultAsync(u => u.логин == login);
    }

    public override async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbSet
            .Include(u => u.роль)
            .Include(u => u.сотрудник)
            .Include(u => u.клиент)
            .ToListAsync();
    }
}

/// <summary>
/// Репозиторий для работы с клиентами
/// </summary>
public class ClientRepository : BaseRepository<Client>, IClientRepository
{
    public ClientRepository(CustomPcDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _dbSet
            .Include(c => c.заказы)
            .ToListAsync();
    }

    public async Task<IEnumerable<Client>> SearchAsync(string searchTerm)
    {
        return await _dbSet
            .Where(c => c.фамилия!.Contains(searchTerm) ||
                       c.имя!.Contains(searchTerm) ||
                       c.email.Contains(searchTerm) ||
                       (c.наименование_организации != null && c.наименование_организации.Contains(searchTerm)))
            .ToListAsync();
    }
}

/// <summary>
/// Репозиторий для работы с заказами
/// </summary>
public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(CustomPcDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _dbSet
            .Include(o => o.клиент)
            .Include(o => o.сборочные_задания)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetByClientIdAsync(int clientId)
    {
        return await _dbSet
            .Where(o => o.клиент_id == clientId)
            .Include(o => o.клиент)
            .Include(o => o.сборочные_задания)
            .ToListAsync();
    }
}

/// <summary>
/// Репозиторий для работы со сборочными заданиями
/// </summary>
public class AssemblyRepository : BaseRepository<AssemblyTask>, IAssemblyRepository
{
    public AssemblyRepository(CustomPcDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<AssemblyTask>> GetAllAsync()
    {
        return await _dbSet
            .Include(a => a.сотрудник)
            .Include(a => a.заказ)
            .Include(a => a.компоненты)
                .ThenInclude(ac => ac.компонент)
            .ToListAsync();
    }

    public async Task<IEnumerable<AssemblyTask>> GetByOrderIdAsync(int orderId)
    {
        return await _dbSet
            .Where(a => a.заказ_id == orderId)
            .Include(a => a.сотрудник)
            .Include(a => a.заказ)
            .Include(a => a.компоненты)
                .ThenInclude(ac => ac.компонент)
            .ToListAsync();
    }

    public async Task<IEnumerable<AssemblyTask>> GetByEmployeeIdAsync(int employeeId)
    {
        return await _dbSet
            .Where(a => a.сотрудник_id == employeeId)
            .Include(a => a.сотрудник)
            .Include(a => a.заказ)
            .Include(a => a.компоненты)
                .ThenInclude(ac => ac.компонент)
            .ToListAsync();
    }
}

/// <summary>
/// Репозиторий для работы с компонентами
/// </summary>
public class ComponentRepository : BaseRepository<Component>, IComponentRepository
{
    public ComponentRepository(CustomPcDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Component>> GetAllAsync()
    {
        return await _dbSet
            .Include(c => c.сборочные_компоненты)
            .Include(c => c.складские_остатки)
            .ToListAsync();
    }

    public async Task<IEnumerable<Component>> SearchAsync(string searchTerm)
    {
        return await _dbSet
            .Where(c => c.наименование.Contains(searchTerm) ||
                       c.тип.Contains(searchTerm) ||
                       c.производитель.Contains(searchTerm))
            .ToListAsync();
    }
}

/// <summary>
/// Репозиторий для работы со складами
/// </summary>
public class WarehouseRepository : BaseRepository<Warehouse>, IWarehouseRepository
{
    public WarehouseRepository(CustomPcDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Warehouse>> GetAllAsync()
    {
        return await _dbSet
            .Include(w => w.остатки)
                .ThenInclude(s => s.компонент)
            .ToListAsync();
    }
}

/// <summary>
/// Репозиторий для работы с остатками на складе
/// </summary>
public class StockRepository : BaseRepository<StockBalance>, IStockRepository
{
    public StockRepository(CustomPcDbContext context) : base(context)
    {
    }

    public async Task<StockBalance?> GetAsync(int componentId, int warehouseId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(sb => sb.компонент_id == componentId && sb.склад_id == warehouseId);
    }

    public async Task<IEnumerable<StockBalance>> GetByComponentIdAsync(int componentId)
    {
        return await _dbSet
            .Where(sb => sb.компонент_id == componentId)
            .Include(sb => sb.склад)
            .ToListAsync();
    }

    public async Task<IEnumerable<StockBalance>> GetByWarehouseIdAsync(int warehouseId)
    {
        return await _dbSet
            .Where(sb => sb.склад_id == warehouseId)
            .Include(sb => sb.компонент)
            .ToListAsync();
    }
}

/// <summary>
/// Репозиторий для работы с сотрудниками
/// </summary>
public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(CustomPcDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _dbSet
            .Include(e => e.отдел)
            .Include(e => e.сборочные_задания)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetByDepartmentIdAsync(int departmentId)
    {
        return await _dbSet
            .Where(e => e.отдел_id == departmentId)
            .Include(e => e.отдел)
            .ToListAsync();
    }
}

/// <summary>
/// Репозиторий для работы с отделами
/// </summary>
public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(CustomPcDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _dbSet
            .Include(d => d.сотрудники)
            .ToListAsync();
    }
}

/// <summary>
/// Репозиторий для работы с ролями
/// </summary>
public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(CustomPcDbContext context) : base(context)
    {
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.название_роли == name);
    }
}

/// <summary>
/// Репозиторий для работы с журналом аудита
/// </summary>
public class AuditRepository : BaseRepository<AuditLog>, IAuditRepository
{
    public AuditRepository(CustomPcDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(int userId)
    {
        return await _dbSet
            .Where(a => a.пользователь_id == userId)
            .OrderByDescending(a => a.дата_изменения)
            .ToListAsync();
    }

    public async Task<IEnumerable<AuditLog>> GetByTableAsync(string tableName)
    {
        return await _dbSet
            .Where(a => a.таблица == tableName)
            .OrderByDescending(a => a.дата_изменения)
            .ToListAsync();
    }

    public async Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(a => a.дата_изменения >= startDate && a.дата_изменения <= endDate)
            .OrderByDescending(a => a.дата_изменения)
            .ToListAsync();
    }
}
