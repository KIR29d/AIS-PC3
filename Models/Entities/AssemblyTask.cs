using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCStoreApp.Models.Entities
{
    [Table("сборочные_задания")]
    public class AssemblyTask
    {
        [Key]
        [Column("сборка_id")]
        public int AssemblyId { get; set; }

        [Required]
        [Column("название_конфигурации")]
        [MaxLength(255)]
        public string ConfigurationName { get; set; } = string.Empty;

        [Required]
        [Column("статус")]
        [MaxLength(50)]
        public string Status { get; set; } = "ожидает"; // ожидает, в_сборке, готово

        [Column("сотрудник_id")]
        public int? EmployeeId { get; set; }

        [Column("заказ_id")]
        public int? OrderId { get; set; }

        [Column("дата_начала")]
        public DateTime? StartDate { get; set; }

        [Column("дата_завершения")]
        public DateTime? CompletionDate { get; set; }

        // Navigation properties
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        public virtual ICollection<AssemblyComponent> AssemblyComponents { get; set; } = new List<AssemblyComponent>();
    }
}