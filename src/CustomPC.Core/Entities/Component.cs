namespace CustomPC.Core.Entities;

/// <summary>
/// Компонент ПК (CPU, GPU, RAM и т.д.)
/// </summary>
public class Component
{
    public int компонент_id { get; set; }
    public string наименование { get; set; } = string.Empty;
    public string тип { get; set; } = string.Empty; // CPU, GPU, RAM, SSD, HDD, PSU, Case, Motherboard, Cooler
    public string производитель { get; set; } = string.Empty;

    // Навигационные свойства
    public virtual ICollection<AssemblyComponent> сборочные_компоненты { get; set; } = new List<AssemblyComponent>();
    public virtual ICollection<StockBalance> складские_остатки { get; set; } = new List<StockBalance>();

    public Component()
    {
    }

    public Component(string наименование, string тип, string производитель)
    {
        this.наименование = наименование;
        this.тип = тип;
        this.производитель = производитель;
    }
}
