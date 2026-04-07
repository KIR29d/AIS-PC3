using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCStoreApp.Models.Entities
{
    [Table("аудит_изменений")]
    public class AuditLog
    {
        [Key]
        [Column("аудит_id")]
        public int AuditId { get; set; }

        [Required]
        [Column("таблица")]
        [MaxLength(100)]
        public string TableName { get; set; } = string.Empty;

        [Required]
        [Column("операция")]
        [MaxLength(10)]
        public string Operation { get; set; } = string.Empty; // INSERT, UPDATE, DELETE

        [Column("пользователь_id")]
        public int UserId { get; set; }

        [Column("дата_изменения")]
        public DateTime ChangeDate { get; set; } = DateTime.Now;

        [Required]
        [Column("детали")]
        public string Details { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}