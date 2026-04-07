using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCStoreApp.Models.Entities
{
    [Table("компоненты_в_сборке")]
    public class AssemblyComponent
    {
        [Column("сборка_id")]
        public int AssemblyId { get; set; }

        [Column("компонент_id")]
        public int ComponentId { get; set; }

        [Required]
        [Column("количество")]
        public int Quantity { get; set; }

        // Navigation properties
        [ForeignKey("AssemblyId")]
        public virtual AssemblyTask AssemblyTask { get; set; } = null!;

        [ForeignKey("ComponentId")]
        public virtual Component Component { get; set; } = null!;
    }
}