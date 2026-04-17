using Microsoft.EntityFrameworkCore;
using CustomPC.Core.Entities;

namespace CustomPC.Infrastructure.Data;

/// <summary>
/// Контекст базы данных АИС CustomPC Manager
/// </summary>
public class CustomPcDbContext : DbContext
{
    public DbSet<Role> роли { get; set; }
    public DbSet<User> пользователи { get; set; }
    public DbSet<Client> клиенты { get; set; }
    public DbSet<Employee> сотрудники { get; set; }
    public DbSet<Department> отделы { get; set; }
    public DbSet<Order> заказы { get; set; }
    public DbSet<AssemblyTask> сборочные_задания { get; set; }
    public DbSet<Component> компоненты { get; set; }
    public DbSet<AssemblyComponent> сборочные_компоненты { get; set; }
    public DbSet<Warehouse> склады { get; set; }
    public DbSet<StockBalance> складские_остатки { get; set; }
    public DbSet<AuditLog> журнал_аудита { get; set; }

    public CustomPcDbContext()
    {
    }

    public CustomPcDbContext(DbContextOptions<CustomPcDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Применение конфигураций из сборки
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomPcDbContext).Assembly);

        // Настройка составных первичных ключей
        modelBuilder.Entity<AssemblyComponent>()
            .HasKey(ac => new { ac.сборка_id, ac.компонент_id });

        modelBuilder.Entity<StockBalance>()
            .HasKey(sb => new { sb.компонент_id, sb.склад_id });

        // Настройка отношений для AssemblyComponent
        modelBuilder.Entity<AssemblyComponent>()
            .HasOne(ac => ac.сборка)
            .WithMany(a => a.компоненты)
            .HasForeignKey(ac => ac.сборка_id);

        modelBuilder.Entity<AssemblyComponent>()
            .HasOne(ac => ac.компонент)
            .WithMany(c => c.сборочные_компоненты)
            .HasForeignKey(ac => ac.компонент_id);

        // Настройка отношений для StockBalance
        modelBuilder.Entity<StockBalance>()
            .HasOne(sb => sb.компонент)
            .WithMany(c => c.складские_остатки)
            .HasForeignKey(sb => sb.компонент_id);

        modelBuilder.Entity<StockBalance>()
            .HasOne(sb => sb.склад)
            .WithMany(w => w.остатки)
            .HasForeignKey(sb => sb.склад_id);

        // Проверочные ограничения
        modelBuilder.Entity<Client>()
            .HasCheckConstraint("CK_клиенты_тип_клиента", "тип_клиента IN ('физическое', 'юридическое')");

        modelBuilder.Entity<Order>()
            .HasCheckConstraint("CK_заказы_статус", 
                "статус IN ('новый', 'в_работе', 'собран', 'отгружен', 'выполнен')");

        modelBuilder.Entity<AssemblyTask>()
            .HasCheckConstraint("CK_сборочные_задания_статус", 
                "статус IN ('ожидает', 'в_сборке', 'готово')");
    }

    public override int SaveChanges()
    {
        AddAuditEntries();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddAuditEntries();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void AddAuditEntries()
    {
        // Здесь будет логика автоматического добавления записей аудита
        // При изменении сущностей
    }
}
