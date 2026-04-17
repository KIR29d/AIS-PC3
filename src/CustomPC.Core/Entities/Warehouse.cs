namespace CustomPC.Core.Entities;

/// <summary>
/// Склад компонентов
/// </summary>
public class Warehouse
{
    public int склад_id { get; set; }
    public string название { get; set; } = string.Empty;
    public string адрес { get; set; } = string.Empty;

    // Навигационные свойства
    public virtual ICollection<StockBalance> остатки { get; set; } = new List<StockBalance>();

    public Warehouse()
    {
    }

    public Warehouse(string название, string адрес)
    {
        this.название = название;
        this.адрес = адрес;
    }
}
