namespace CustomPC.Core.Entities;

/// <summary>
/// Компонент в сборочном задании (промежуточная таблица)
/// </summary>
public class AssemblyComponent
{
    public int сборка_id { get; set; }
    public int компонент_id { get; set; }
    public int количество { get; set; }

    // Навигационные свойства
    public virtual AssemblyTask сборка { get; set; } = null!;
    public virtual Component компонент { get; set; } = null!;

    public AssemblyComponent()
    {
    }

    public AssemblyComponent(int сборка_id, int компонент_id, int количество)
    {
        this.сборка_id = сборка_id;
        this.компонент_id = компонент_id;
        this.количество = количество;
    }

    public bool ValidateQuantity()
    {
        return количество > 0;
    }
}
