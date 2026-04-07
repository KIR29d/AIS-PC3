using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCStoreApp.Models.Entities
{
    [Table("клиенты")]
    public class Client
    {
        [Key]
        [Column("клиент_id")]
        public int ClientId { get; set; }

        [Required]
        [Column("тип_клиента")]
        [MaxLength(20)]
        public string ClientType { get; set; } = string.Empty; // физическое, юридическое

        [Column("фамилия")]
        [MaxLength(100)]
        public string? LastName { get; set; }

        [Column("имя")]
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [Column("отчество")]
        [MaxLength(100)]
        public string? MiddleName { get; set; }

        [Column("наименование_организации")]
        [MaxLength(255)]
        public string? OrganizationName { get; set; }

        [Required]
        [Column("email")]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("номер_телефона")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Column("дата_регистрации")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}