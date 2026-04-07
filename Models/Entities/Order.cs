using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCStoreApp.Models.Entities
{
    [Table("заказы")]
    public class Order
    {
        [Key]
        [Column("заказ_id")]
        public int OrderId { get; set; }

        [Column("клиент_id")]
        public int ClientId { get; set; }

        [Required]
        [Column("статус")]
        [MaxLength(50)]
        public string Status { get; set; } = "новый"; // новый, в_работе, собран, отгружен, выполнен

        [Column("общая_сумма", TypeName = "decimal(12,2)")]
        public decimal TotalAmount { get; set; } = 0.00m;

        [Column("дата_создания")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Column("дата_отгрузки")]
        public DateTime? ShipmentDate { get; set; }

        // Navigation properties
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; } = null!;
        public virtual ICollection<AssemblyTask> AssemblyTasks { get; set; } = new List<AssemblyTask>();
    }
}