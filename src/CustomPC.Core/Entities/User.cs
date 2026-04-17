namespace CustomPC.Core.Entities;

/// <summary>
/// Пользователь системы
/// </summary>
public class User
{
    public int пользователь_id { get; set; }
    public string логин { get; set; } = string.Empty;
    public string пароль { get; set; } = string.Empty;
    public int роль_id { get; set; }
    public int? сотрудник_id { get; set; }
    public int? клиент_id { get; set; }
    public bool активен { get; set; } = true;
    public DateTime дата_создания { get; set; } = DateTime.UtcNow;
    public DateTime? дата_последнего_входа { get; set; }

    // Навигационные свойства
    public virtual Role? роль { get; set; }
    public virtual Employee? сотрудник { get; set; }
    public virtual Client? клиент { get; set; }

    public User()
    {
    }

    public bool Authenticate(string password)
    {
        return пароль == password; // В реальной системе использовать хеширование
    }

    public void UpdateLastLogin()
    {
        дата_последнего_входа = DateTime.UtcNow;
    }

    public bool IsActive()
    {
        return активен;
    }
}
