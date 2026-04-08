using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomPcStoreApp.Models;

/// <summary>
/// Модель остатков на складе (связующая таблица)
/// </summary>
[Table("остатки_на_складе")]
public class Stock
{
    [Key, Column("компонент_id", Order = 0)]
    public int ComponentId { get; set; }

    [Key, Column("склад_id", Order = 1)]
    public int WarehouseId { get; set; }

    [Required]
    [Column("количество")]
    public int Quantity { get; set; }

    [Column("дата_обновления")]
    public DateTime UpdateDate { get; set; } = DateTime.Now;

    // Навигационные свойства
    [ForeignKey(nameof(ComponentId))]
    public virtual Component Component { get; set; } = null!;

    [ForeignKey(nameof(WarehouseId))]
    public virtual Warehouse Warehouse { get; set; } = null!;
}