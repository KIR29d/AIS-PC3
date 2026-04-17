namespace CustomPC.Core.Entities;

/// <summary>
/// Сотрудник компании
/// </summary>
public class Employee
{
    public int сотрудник_id { get; set; }
    public string фамилия { get; set; } = string.Empty;
    public string имя { get; set; } = string.Empty;
    public string? отчество { get; set; }
    public string должность { get; set; } = string.Empty;
    public DateOnly дата_приёма { get; set; }
    public int отдел_id { get; set; }

    // Навигационные свойства
    public virtual Department отдел { get; set; } = null!;
    public virtual ICollection<AssemblyTask> сборочные_задания { get; set; } = new List<AssemblyTask>();

    public Employee()
    {
    }

    public string GetFullName()
    {
        return $"{фамилия} {имя} {отчество}".Trim();
    }

    public int YearsEmployed()
    {
        return DateTime.Now.Year - дата_приёма.Year;
    }
}
