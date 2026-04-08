using Microsoft.EntityFrameworkCore;
using CustomPcStoreApp.Models;

namespace CustomPcStoreApp.Data;

/// <summary>
/// Контекст базы данных приложения
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSets для всех сущностей
    public DbSet<Role> Roles { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<AssemblyTask> AssemblyTasks { get; set; }
    public DbSet<AssemblyComponent> AssemblyComponents { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Настройка составных ключей
        modelBuilder.Entity<AssemblyComponent>()
            .HasKey(ac => new { ac.AssemblyTaskId, ac.ComponentId });

        modelBuilder.Entity<Stock>()
            .HasKey(s => new { s.ComponentId, s.WarehouseId });

        // Настройка индексов
        modelBuilder.Entity<Component>()
            .HasIndex(c => c.Type)
            .HasDatabaseName("idx_компоненты_тип");

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.StatusString)
            .HasDatabaseName("idx_заказы_статус");

        modelBuilder.Entity<AssemblyTask>()
            .HasIndex(at => at.StatusString)
            .HasDatabaseName("idx_сборочные_статус");

        modelBuilder.Entity<AuditLog>()
            .HasIndex(al => al.ChangeDate)
            .HasDatabaseName("idx_аудит_дата");

        // Настройка уникальных ключей
        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique()
            .HasDatabaseName("uk_роли_название");

        modelBuilder.Entity<Client>()
            .HasIndex(c => c.Email)
            .IsUnique()
            .HasDatabaseName("uk_клиенты_email");

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique()
            .HasDatabaseName("uk_пользователи_логин");

        // Настройка ограничений CHECK (будут применены на уровне БД)
        modelBuilder.Entity<Client>()
            .ToTable(t => t.HasCheckConstraint("CK_клиенты_тип", "тип_клиента IN ('физическое', 'юридическое')"));

        modelBuilder.Entity<Order>()
            .ToTable(t => t.HasCheckConstraint("CK_заказы_статус", "статус IN ('новый', 'в_работе', 'собран', 'отгружен', 'выполнен')"));

        modelBuilder.Entity<AssemblyTask>()
            .ToTable(t => t.HasCheckConstraint("CK_сборочные_статус", "статус IN ('ожидает', 'в_сборке', 'готово')"));

        modelBuilder.Entity<Stock>()
            .ToTable(t => t.HasCheckConstraint("CK_остатки_количество", "количество >= 0"));

        modelBuilder.Entity<AuditLog>()
            .ToTable(t => t.HasCheckConstraint("CK_аудит_операция", "операция IN ('INSERT', 'UPDATE', 'DELETE')"));

        // Настройка каскадного удаления
        modelBuilder.Entity<AssemblyComponent>()
            .HasOne(ac => ac.AssemblyTask)
            .WithMany(at => at.AssemblyComponents)
            .HasForeignKey(ac => ac.AssemblyTaskId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AssemblyComponent>()
            .HasOne(ac => ac.Component)
            .WithMany(c => c.AssemblyComponents)
            .HasForeignKey(ac => ac.ComponentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Stock>()
            .HasOne(s => s.Component)
            .WithMany(c => c.Stocks)
            .HasForeignKey(s => s.ComponentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Stock>()
            .HasOne(s => s.Warehouse)
            .WithMany(w => w.Stocks)
            .HasForeignKey(s => s.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Настройка ограничений внешних ключей
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Employee)
            .WithMany(e => e.Users)
            .HasForeignKey(u => u.EmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Client)
            .WithMany(c => c.Users)
            .HasForeignKey(u => u.ClientId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Client)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AssemblyTask>()
            .HasOne(at => at.Employee)
            .WithMany(e => e.AssemblyTasks)
            .HasForeignKey(at => at.EmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<AssemblyTask>()
            .HasOne(at => at.Order)
            .WithMany(o => o.AssemblyTasks)
            .HasForeignKey(at => at.OrderId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<AuditLog>()
            .HasOne(al => al.User)
            .WithMany(u => u.AuditLogs)
            .HasForeignKey(al => al.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Настройка значений по умолчанию
        modelBuilder.Entity<Client>()
            .Property(c => c.RegistrationDate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<User>()
            .Property(u => u.CreatedDate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<User>()
            .Property(u => u.IsActive)
            .HasDefaultValue(true);

        modelBuilder.Entity<Order>()
            .Property(o => o.CreatedDate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .HasDefaultValue(0.00m);

        modelBuilder.Entity<Stock>()
            .Property(s => s.UpdateDate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<AuditLog>()
            .Property(al => al.ChangeDate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}