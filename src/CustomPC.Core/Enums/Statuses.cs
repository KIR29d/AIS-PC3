namespace CustomPC.Core.Enums;

/// <summary>
/// Статус заказа
/// </summary>
public enum OrderStatus
{
    новый,
    в_работе,
    собран,
    отгружен,
    выполнен
}

/// <summary>
/// Статус сборочного задания
/// </summary>
public enum AssemblyStatus
{
    ожидает,
    в_сборке,
    готово
}

/// <summary>
/// Тип клиента
/// </summary>
public enum ClientType
{
    физическое,
    юридическое
}

/// <summary>
/// Операция аудита
/// </summary>
public enum AuditOperation
{
    INSERT,
    UPDATE,
    DELETE
}
