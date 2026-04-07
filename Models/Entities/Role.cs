using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCStoreApp.Models.Entities
{
    [Table("роли")]
    public class Role
    {
        [Key]
        [Column("роль_id")]
        public int RoleId { get; set; }

        [Required]
        [Column("название_роли")]
        [MaxLength(50)]
        public string RoleName { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}