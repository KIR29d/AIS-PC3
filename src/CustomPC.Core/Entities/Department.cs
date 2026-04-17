namespace CustomPC.Core.Entities;

/// <summary>
/// Отдел компании
/// </summary>
public class Department
{
    public int отдел_id { get; set; }
    public string название { get; set; } = string.Empty;

    // Навигационные свойства
    public virtual ICollection<Employee> сотрудники { get; set; } = new List<Employee>();

    public Department()
    {
    }

    public Department(string название)
    {
        this.название = название;
    }
}
