using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCStoreApp.Models.Entities
{
    [Table("склады")]
    public class Warehouse
    {
        [Key]
        [Column("склад_id")]
        public int WarehouseId { get; set; }

        [Required]
        [Column("название")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("адрес")]
        public string Address { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
    }
}