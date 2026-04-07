using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCStoreApp.Models.Entities
{
    [Table("остатки_на_складе")]
    public class InventoryItem
    {
        [Column("компонент_id")]
        public int ComponentId { get; set; }

        [Column("склад_id")]
        public int WarehouseId { get; set; }

        [Required]
        [Column("количество")]
        public int Quantity { get; set; }

        [Column("дата_обновления")]
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("ComponentId")]
        public virtual Component Component { get; set; } = null!;

        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouse { get; set; } = null!;
    }
}