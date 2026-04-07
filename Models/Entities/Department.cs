using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCStoreApp.Models.Entities
{
    [Table("отделы")]
    public class Department
    {
        [Key]
        [Column("отдел_id")]
        public int DepartmentId { get; set; }

        [Required]
        [Column("название")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}