using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomPcStoreApp.Models;

/// <summary>
/// Модель сборочного задания
/// </summary>
[Table("сборочные_задания")]
public class AssemblyTask : BaseEntity
{
    [Key]
    [Column("сборка_id")]
    public override int Id { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("название_конфигурации")]
    public string ConfigurationName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("статус")]
    public string StatusString { get; set; } = "ожидает";

    [NotMapped]
    public AssemblyStatus Status
    {
        get => StatusString switch
        {
            "ожидает" => AssemblyStatus.Waiting,
            "в_сборке" => AssemblyStatus.InProgress,
            "готово" => AssemblyStatus.Ready,
            _ => AssemblyStatus.Waiting
        };
        set => StatusString = value switch
        {
            AssemblyStatus.Waiting => "ожидает",
            AssemblyStatus.InProgress => "в_сборке",
            AssemblyStatus.Ready => "готово",
            _ => "ожидает"
        };
    }

    [Column("сотрудник_id")]
    public int? EmployeeId { get; set; }

    [Column("заказ_id")]
    public int? OrderId { get; set; }

    [Column("дата_начала")]
    public DateTime? StartDate { get; set; }

    [Column("дата_завершения")]
    public DateTime? CompletionDate { get; set; }

    // Навигационные свойства
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee? Employee { get; set; }

    [ForeignKey(nameof(OrderId))]
    public virtual Order? Order { get; set; }

    public virtual ICollection<AssemblyComponent> AssemblyComponents { get; set; } = new List<AssemblyComponent>();
}