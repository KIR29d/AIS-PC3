using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomPcStoreApp.Models;

/// <summary>
/// Модель компонента ПК
/// </summary>
[Table("компоненты")]
public class Component : BaseEntity
{
    [Key]
    [Column("компонент_id")]
    public override int Id { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("наименование")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("тип")]
    public string Type { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("производитель")]
    public string Manufacturer { get; set; } = string.Empty;

    // Навигационные свойства
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    public virtual ICollection<AssemblyComponent> AssemblyComponents { get; set; } = new List<AssemblyComponent>();
}