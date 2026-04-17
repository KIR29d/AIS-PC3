namespace CustomPC.Core.Entities;

/// <summary>
/// Остаток компонента на складе
/// </summary>
public class StockBalance
{
    public int компонент_id { get; set; }
    public int склад_id { get; set; }
    public int количество { get; set; }
    public DateTime дата_обновления { get; set; } = DateTime.UtcNow;

    // Навигационные свойства
    public virtual Component компонент { get; set; } = null!;
    public virtual Warehouse склад { get; set; } = null!;

    public StockBalance()
    {
    }

    public void UpdateQuantity(int newQuantity)
    {
        количество = newQuantity;
        дата_обновления = DateTime.UtcNow;
    }

    public void AddQuantity(int amount)
    {
        количество += amount;
        дата_обновления = DateTime.UtcNow;
    }

    public bool RemoveQuantity(int amount)
    {
        if (количество >= amount)
        {
            количество -= amount;
            дата_обновления = DateTime.UtcNow;
            return true;
        }
        return false;
    }
}
