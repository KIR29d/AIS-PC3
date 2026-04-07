using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCStoreApp.Models.Entities
{
    [Table("компоненты")]
    public class Component
    {
        [Key]
        [Column("компонент_id")]
        public int ComponentId { get; set; }

        [Required]
        [Column("наименование")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("тип")]
        [MaxLength(50)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Column("производитель")]
        [MaxLength(100)]
        public string Manufacturer { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
        public virtual ICollection<AssemblyComponent> AssemblyComponents { get; set; } = new List<AssemblyComponent>();
    }
}