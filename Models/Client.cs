using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomPcStoreApp.Models;

/// <summary>
/// Модель клиента
/// </summary>
[Table("клиенты")]
public class Client : BaseEntity
{
    [Key]
    [Column("клиент_id")]
    public override int Id { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("тип_клиента")]
    public string ClientTypeString { get; set; } = string.Empty;

    [NotMapped]
    public ClientType ClientType
    {
        get => ClientTypeString == "физическое" ? ClientType.Individual : ClientType.Legal;
        set => ClientTypeString = value == ClientType.Individual ? "физическое" : "юридическое";
    }

    [MaxLength(100)]
    [Column("фамилия")]
    public string? LastName { get; set; }

    [MaxLength(100)]
    [Column("имя")]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    [Column("отчество")]
    public string? MiddleName { get; set; }

    [MaxLength(255)]
    [Column("наименование_организации")]
    public string? OrganizationName { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    [Column("номер_телефона")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Column("дата_регистрации")]
    public DateTime RegistrationDate { get; set; } = DateTime.Now;

    // Навигационные свойства
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<User> Users { get; set; } = new List<User>();

    [NotMapped]
    public string DisplayName => ClientType == ClientType.Individual 
        ? $"{LastName} {FirstName} {MiddleName}".Trim()
        : OrganizationName ?? string.Empty;
}