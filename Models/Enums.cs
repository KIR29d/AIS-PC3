namespace CustomPcStoreApp.Models;

/// <summary>
/// Тип клиента
/// </summary>
public enum ClientType
{
    Individual, // физическое лицо
    Legal       // юридическое лицо
}

/// <summary>
/// Статус заказа
/// </summary>
public enum OrderStatus
{
    New,        // новый
    InProgress, // в_работе
    Assembled,  // собран
    Shipped,    // отгружен
    Completed   // выполнен
}

/// <summary>
/// Статус сборочного задания
/// </summary>
public enum AssemblyStatus
{
    Waiting,    // ожидает
    InProgress, // в_сборке
    Ready       // готово
}

/// <summary>
/// Роли пользователей
/// </summary>
public enum UserRole
{
    Administrator = 1,
    Manager = 2,
    Assembler = 3,
    Client = 4
}