using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomPcStoreApp.Models;

/// <summary>
/// Модель отдела
/// </summary>
[Table("отделы")]
public class Department : BaseEntity
{
    [Key]
    [Column("отдел_id")]
    public override int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("название")]
    public string Name { get; set; } = string.Empty;

    // Навигационные свойства
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}