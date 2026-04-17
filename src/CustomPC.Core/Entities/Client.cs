namespace CustomPC.Core.Entities;

/// <summary>
/// Клиент (физическое или юридическое лицо)
/// </summary>
public class Client
{
    public int клиент_id { get; set; }
    public string тип_клиента { get; set; } = "физическое"; // "физическое" или "юридическое"
    public string? фамилия { get; set; }
    public string? имя { get; set; }
    public string? отчество { get; set; }
    public string? наименование_организации { get; set; }
    public string email { get; set; } = string.Empty;
    public string номер_телефона { get; set; } = string.Empty;
    public DateTime дата_регистрации { get; set; } = DateTime.UtcNow;

    // Навигационные свойства
    public virtual ICollection<Order> заказы { get; set; } = new List<Order>();

    public Client()
    {
    }

    public bool IsIndividual()
    {
        return тип_клиента == "физическое";
    }

    public bool IsLegalEntity()
    {
        return тип_клиента == "юридическое";
    }

    public string GetFullName()
    {
        if (IsIndividual())
        {
            return $"{фамилия} {имя} {отчество}".Trim();
        }
        return наименование_организации ?? string.Empty;
    }

    public bool ValidateEmail()
    {
        return !string.IsNullOrWhiteSpace(email) && email.Contains("@");
    }

    public bool ValidatePhone()
    {
        return !string.IsNullOrWhiteSpace(номер_телефона);
    }
}
