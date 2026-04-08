using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomPcStoreApp.Models;

/// <summary>
/// Модель сотрудника
/// </summary>
[Table("сотрудники")]
public class Employee : BaseEntity
{
    [Key]
    [Column("сотрудник_id")]
    public override int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("фамилия")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("имя")]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(100)]
    [Column("отчество")]
    public string? MiddleName { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("должность")]
    public string Position { get; set; } = string.Empty;

    [Column("дата_приёма")]
    public DateTime HireDate { get; set; }

    [Column("отдел_id")]
    public int DepartmentId { get; set; }

    // Навигационные свойства
    [ForeignKey(nameof(DepartmentId))]
    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public virtual ICollection<AssemblyTask> AssemblyTasks { get; set; } = new List<AssemblyTask>();

    [NotMapped]
    public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
}