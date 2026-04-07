using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCStoreApp.Models.Entities
{
    [Table("пользователи")]
    public class User
    {
        [Key]
        [Column("пользователь_id")]
        public int UserId { get; set; }

        [Required]
        [Column("логин")]
        [MaxLength(100)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [Column("пароль")]
        [MaxLength(255)]
        public string Password { get; set; } = string.Empty;

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

        // Navigation properties
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } = null!;

        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client? Client { get; set; }

        public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }
}