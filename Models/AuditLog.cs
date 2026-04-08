using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomPcStoreApp.Models;

/// <summary>
/// Модель журнала аудита
/// </summary>
[Table("аудит_изменений")]
public class AuditLog : BaseEntity
{
    [Key]
    [Column("аудит_id")]
    public override int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("таблица")]
    public string TableName { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    [Column("операция")]
    public string Operation { get; set; } = string.Empty;

    [Column("пользователь_id")]
    public int UserId { get; set; }

    [Column("дата_изменения")]
    public DateTime ChangeDate { get; set; } = DateTime.Now;

    [Required]
    [Column("детали")]
    public string Details { get; set; } = string.Empty;

    // Навигационные свойства
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;
}