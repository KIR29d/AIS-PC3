using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCStoreApp.Models.Entities
{
    [Table("сотрудники")]
    public class Employee
    {
        [Key]
        [Column("сотрудник_id")]
        public int EmployeeId { get; set; }

        [Required]
        [Column("фамилия")]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Column("имя")]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Column("отчество")]
        [MaxLength(100)]
        public string? MiddleName { get; set; }

        [Required]
        [Column("должность")]
        [MaxLength(100)]
        public string Position { get; set; } = string.Empty;

        [Column("дата_приёма")]
        public DateTime HireDate { get; set; }

        [Column("отдел_id")]
        public int DepartmentId { get; set; }

        // Navigation properties
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; } = null!;
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<AssemblyTask> AssemblyTasks { get; set; } = new List<AssemblyTask>();

        public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
    }
}