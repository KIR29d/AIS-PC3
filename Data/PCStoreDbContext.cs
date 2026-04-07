using Microsoft.EntityFrameworkCore;
using PCStoreApp.Models.Entities;

namespace PCStoreApp.Data
{
    public class PCStoreDbContext : DbContext
    {
        public PCStoreDbContext(DbContextOptions<PCStoreDbContext> options) : base(options)
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
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка составных ключей
            modelBuilder.Entity<AssemblyComponent>()
                .HasKey(ac => new { ac.AssemblyId, ac.ComponentId });

            modelBuilder.Entity<InventoryItem>()
                .HasKey(ii => new { ii.ComponentId, ii.WarehouseId });

            // Настройка ограничений CHECK для MySQL
            modelBuilder.Entity<Client>()
                .HasCheckConstraint("CK_Client_Type", "тип_клиента IN ('физическое', 'юридическое')");

            modelBuilder.Entity<Order>()
                .HasCheckConstraint("CK_Order_Status", "статус IN ('новый', 'в_работе', 'собран', 'отгружен', 'выполнен')");

            modelBuilder.Entity<AssemblyTask>()
                .HasCheckConstraint("CK_Assembly_Status", "статус IN ('ожидает', 'в_сборке', 'готово')");

            modelBuilder.Entity<AuditLog>()
                .HasCheckConstraint("CK_Audit_Operation", "операция IN ('INSERT', 'UPDATE', 'DELETE')");

            modelBuilder.Entity<InventoryItem>()
                .HasCheckConstraint("CK_Inventory_Quantity", "количество >= 0");

            // Настройка индексов
            modelBuilder.Entity<Component>()
                .HasIndex(c => c.Type)
                .HasDatabaseName("idx_компоненты_тип");

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.Status)
                .HasDatabaseName("idx_заказы_статус");

            modelBuilder.Entity<AssemblyTask>()
                .HasIndex(at => at.Status)
                .HasDatabaseName("idx_сборочные_статус");

            modelBuilder.Entity<AuditLog>()
                .HasIndex(al => al.ChangeDate)
                .HasDatabaseName("idx_аудит_дата");

            // Настройка уникальных ключей
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleName)
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
        }
    }
}