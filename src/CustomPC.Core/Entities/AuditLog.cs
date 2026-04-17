namespace CustomPC.Core.Entities;

/// <summary>
/// Запись аудита изменений в системе
/// </summary>
public class AuditLog
{
    public int аудит_id { get; set; }
    public string таблица { get; set; } = string.Empty;
    public string операция { get; set; } = string.Empty; // INSERT, UPDATE, DELETE
    public int пользователь_id { get; set; }
    public DateTime дата_изменения { get; set; } = DateTime.UtcNow;
    public string детали { get; set; } = string.Empty;

    // Навигационные свойства
    public virtual User пользователь { get; set; } = null!;

    public AuditLog()
    {
    }

    public void LogInsert(string tableName, int userId, string details)
    {
        таблица = tableName;
        операция = "INSERT";
        пользователь_id = userId;
        детали = details;
        дата_изменения = DateTime.UtcNow;
    }

    public void LogUpdate(string tableName, int userId, string details)
    {
        таблица = tableName;
        операция = "UPDATE";
        пользователь_id = userId;
        детали = details;
        дата_изменения = DateTime.UtcNow;
    }

    public void LogDelete(string tableName, int userId, string details)
    {
        таблица = tableName;
        операция = "DELETE";
        пользователь_id = userId;
        детали = details;
        дата_изменения = DateTime.UtcNow;
    }
}
