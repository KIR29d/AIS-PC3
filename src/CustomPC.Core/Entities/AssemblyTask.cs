namespace CustomPC.Core.Entities;

/// <summary>
/// Сборочное задание
/// </summary>
public class AssemblyTask
{
    public int сборка_id { get; set; }
    public string название_конфигурации { get; set; } = string.Empty;
    public string статус { get; set; } = "ожидает"; // ожидает, в_сборке, готово
    public int? сотрудник_id { get; set; }
    public int? заказ_id { get; set; }
    public DateTime? дата_начала { get; set; }
    public DateTime? дата_завершения { get; set; }

    // Навигационные свойства
    public virtual Employee? сотрудник { get; set; }
    public virtual Order? заказ { get; set; }
    public virtual ICollection<AssemblyComponent> компоненты { get; set; } = new List<AssemblyComponent>();

    public AssemblyTask()
    {
    }

    public void StartAssembly(int employeeId)
    {
        сотрудник_id = employeeId;
        статус = "в_сборке";
        дата_начала = DateTime.UtcNow;
    }

    public void CompleteAssembly()
    {
        статус = "готово";
        дата_завершения = DateTime.UtcNow;
    }
}
