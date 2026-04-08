using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomPcStoreApp.Models;

/// <summary>
/// Модель склада
/// </summary>
[Table("склады")]
public class Warehouse : BaseEntity
{
    [Key]
    [Column("склад_id")]
    public override int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("название")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column("адрес")]
    public string Address { get; set; } = string.Empty;

    // Навигационные свойства
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}