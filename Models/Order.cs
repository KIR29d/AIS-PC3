using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomPcStoreApp.Models;

/// <summary>
/// Модель заказа
/// </summary>
[Table("заказы")]
public class Order : BaseEntity
{
    [Key]
    [Column("заказ_id")]
    public override int Id { get; set; }

    [Column("клиент_id")]
    public int ClientId { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("статус")]
    public string StatusString { get; set; } = "новый";

    [NotMapped]
    public OrderStatus Status
    {
        get => StatusString switch
        {
            "новый" => OrderStatus.New,
            "в_работе" => OrderStatus.InProgress,
            "собран" => OrderStatus.Assembled,
            "отгружен" => OrderStatus.Shipped,
            "выполнен" => OrderStatus.Completed,
            _ => OrderStatus.New
        };
        set => StatusString = value switch
        {
            OrderStatus.New => "новый",
            OrderStatus.InProgress => "в_работе",
            OrderStatus.Assembled => "собран",
            OrderStatus.Shipped => "отгружен",
            OrderStatus.Completed => "выполнен",
            _ => "новый"
        };
    }

    [Column("общая_сумма")]
    public decimal TotalAmount { get; set; } = 0.00m;

    [Column("дата_создания")]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [Column("дата_отгрузки")]
    public DateTime? ShippingDate { get; set; }

    // Навигационные свойства
    [ForeignKey(nameof(ClientId))]
    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<AssemblyTask> AssemblyTasks { get; set; } = new List<AssemblyTask>();
}