using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomPcStoreApp.Models;

/// <summary>
/// Модель роли пользователя
/// </summary>
[Table("роли")]
public class Role : BaseEntity
{
    [Key]
    [Column("роль_id")]
    public override int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("название_роли")]
    public string Name { get; set; } = string.Empty;

    // Навигационные свойства
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}