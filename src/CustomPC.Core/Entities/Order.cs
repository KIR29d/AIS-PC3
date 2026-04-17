namespace CustomPC.Core.Entities;

/// <summary>
/// Заказ клиента
/// </summary>
public class Order
{
    public int заказ_id { get; set; }
    public int клиент_id { get; set; }
    public string статус { get; set; } = "новый"; // новый, в_работе, собран, отгружен, выполнен
    public decimal общая_сумма { get; set; }
    public DateTime дата_создания { get; set; } = DateTime.UtcNow;
    public DateTime? дата_отгрузки { get; set; }

    // Навигационные свойства
    public virtual Client клиент { get; set; } = null!;
    public virtual ICollection<AssemblyTask> сборочные_задания { get; set; } = new List<AssemblyTask>();

    public Order()
    {
    }

    public void ChangeStatus(string newStatus)
    {
        статус = newStatus;
        if (newStatus == "отгружен" && дата_отгрузки == null)
        {
            дата_отгрузки = DateTime.UtcNow;
        }
    }

    public bool CanShip()
    {
        return статус == "собран";
    }

    public void Ship()
    {
        if (CanShip())
        {
            ChangeStatus("отгружен");
        }
    }
}
