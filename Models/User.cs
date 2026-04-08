using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomPcStoreApp.Models;

/// <summary>
/// Модель пользователя системы
/// </summary>
[Table("пользователи")]
public class User : BaseEntity
{
    [Key]
    [Column("пользователь_id")]
    public override int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("логин")]
    public string Login { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("пароль")]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("роль_id")]
    public int RoleId { get; set; }

    [Column("сотрудник_id")]
    public int? EmployeeId { get; set; }

    [Column("клиент_id")]
    public int? ClientId { get; set; }

    [Column("активен")]
    public bool IsActive { get; set; } = true;

    [Column("дата_создания")]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [Column("дата_последнего_входа")]
    public DateTime? LastLoginDate { get; set; }

    // Навигационные свойства
    [ForeignKey(nameof(RoleId))]
    public virtual Role Role { get; set; } = null!;

    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee? Employee { get; set; }

    [ForeignKey(nameof(ClientId))]
    public virtual Client? Client { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    [NotMapped]
    public string DisplayName => Employee?.FullName ?? Client?.DisplayName ?? Login;
}