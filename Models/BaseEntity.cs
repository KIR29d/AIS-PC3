using System.ComponentModel.DataAnnotations;

namespace CustomPcStoreApp.Models;

/// <summary>
/// Базовая модель для всех сущностей
/// </summary>
public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime? UpdatedAt { get; set; }
}