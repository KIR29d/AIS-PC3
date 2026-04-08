using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomPcStoreApp.Models;

/// <summary>
/// Модель компонентов в сборке (связующая таблица)
/// </summary>
[Table("компоненты_в_сборке")]
public class AssemblyComponent
{
    [Key, Column("сборка_id", Order = 0)]
    public int AssemblyTaskId { get; set; }

    [Key, Column("компонент_id", Order = 1)]
    public int ComponentId { get; set; }

    [Required]
    [Column("количество")]
    public int Quantity { get; set; }

    // Навигационные свойства
    [ForeignKey(nameof(AssemblyTaskId))]
    public virtual AssemblyTask AssemblyTask { get; set; } = null!;

    [ForeignKey(nameof(ComponentId))]
    public virtual Component Component { get; set; } = null!;
}