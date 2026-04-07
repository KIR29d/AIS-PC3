# Дизайн десктопного приложения АИС магазина кастомных ПК

## Обзор

Десктопное приложение будет разработано на C# с использованием WPF для создания современного пользовательского интерфейса. Приложение будет подключаться к MySQL базе данных через Entity Framework Core и обеспечивать ролевой доступ к функциональности.

## Архитектура

### Архитектурный паттерн
Приложение будет использовать архитектуру MVVM (Model-View-ViewModel) с следующими слоями:

- **Presentation Layer (WPF Views)** - пользовательский интерфейс
- **ViewModel Layer** - логика представления и привязка данных
- **Business Logic Layer** - бизнес-логика и сервисы
- **Data Access Layer** - работа с базой данных через Entity Framework Core
- **Database Layer** - MySQL база данных

### Технологический стек
- **.NET 6/7** - основная платформа
- **WPF** - пользовательский интерфейс
- **Entity Framework Core** - ORM для работы с БД
- **MySQL Connector** - подключение к MySQL
- **BCrypt.Net** - хеширование паролей
- **Prism или CommunityToolkit.Mvvm** - MVVM фреймворк

## Компоненты и интерфейсы

### Основные модули

#### 1. Модуль авторизации
- **LoginWindow** - окно входа в систему
- **AuthenticationService** - сервис аутентификации
- **UserSession** - управление сессией пользователя

#### 2. Главное окно
- **MainWindow** - основное окно приложения
- **NavigationService** - навигация между модулями
- **MenuService** - управление меню в зависимости от роли

#### 3. Модуль управления заказами
- **OrdersView** - список заказов
- **OrderDetailsView** - детали заказа
- **CreateOrderView** - создание нового заказа
- **OrderService** - бизнес-логика заказов

#### 4. Модуль сборочных заданий
- **AssemblyTasksView** - список сборочных заданий
- **AssemblyDetailsView** - детали сборки
- **AssemblyService** - управление сборочными заданиями

#### 5. Модуль складского учета
- **InventoryView** - управление остатками
- **ComponentsView** - справочник компонентов
- **InventoryService** - логика складского учета

#### 6. Модуль администрирования
- **UsersManagementView** - управление пользователями
- **AuditLogView** - журнал аудита
- **AdminService** - административные функции

### Интерфейсы сервисов

```csharp
public interface IAuthenticationService
{
    Task<User> AuthenticateAsync(string login, string password);
    void Logout();
    User CurrentUser { get; }
}

public interface IOrderService
{
    Task<List<Order>> GetOrdersAsync();
    Task<Order> CreateOrderAsync(Order order);
    Task UpdateOrderStatusAsync(int orderId, string status);
}

public interface IAuditService
{
    Task LogChangeAsync(string table, string operation, int userId, string details);
}
```

## Модели данных

### Основные Entity классы

```csharp
public class User
{
    public int UserId { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public bool IsActive { get; set; }
}

public class Order
{
    public int OrderId { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; }
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class AssemblyTask
{
    public int AssemblyId { get; set; }
    public string ConfigurationName { get; set; }
    public string Status { get; set; }
    public int? EmployeeId { get; set; }
    public Employee Employee { get; set; }
}
```

## Дизайн пользовательского интерфейса

### Цветовая схема (серые тона)
- **Основной фон**: #F5F5F5 (светло-серый)
- **Панели**: #E0E0E0 (средне-серый)
- **Границы**: #BDBDBD (темно-серый)
- **Текст**: #424242 (темно-серый)
- **Акценты**: #757575 (серый)
- **Кнопки**: #9E9E9E (серый) / #616161 (при наведении)

### Структура главного окна
```
┌─────────────────────────────────────────────────────────┐
│ Меню: Файл | Заказы | Сборка | Склад | Администрирование │
├─────────────────────────────────────────────────────────┤
│ Панель инструментов: [Новый] [Обновить] [Настройки]     │
├─────────────────────────────────────────────────────────┤
│                                                         │
│              Основная рабочая область                   │
│                  (ContentControl)                       │
│                                                         │
├─────────────────────────────────────────────────────────┤
│ Статус: Пользователь: [Имя] | Роль: [Роль] | Время     │
└─────────────────────────────────────────────────────────┘
```

### Заглушки изображений
- **Логотип компании**: Прямоугольник с текстом "ЛОГОТИП"
- **Фото компонентов**: Квадрат с текстом "ФОТО КОМПОНЕНТА"
- **Иконки**: Простые геометрические фигуры с подписями

## Обработка ошибок

### Стратегия обработки ошибок
1. **Глобальный обработчик** - перехват необработанных исключений
2. **Логирование** - запись ошибок в файл и базу данных
3. **Пользовательские сообщения** - понятные сообщения об ошибках
4. **Валидация данных** - проверка на уровне ViewModel и сервисов

### Типы ошибок
- **Ошибки подключения к БД** - уведомление пользователя, попытка переподключения
- **Ошибки валидации** - подсветка полей, сообщения рядом с полями
- **Ошибки авторизации** - блокировка доступа, перенаправление на форму входа
- **Системные ошибки** - логирование, общее сообщение пользователю

## Стратегия тестирования

### Типы тестов
1. **Unit тесты** - тестирование сервисов и ViewModels
2. **Integration тесты** - тестирование работы с базой данных
3. **UI тесты** - автоматизированное тестирование интерфейса

### Инструменты тестирования
- **xUnit** - фреймворк для unit тестов
- **Moq** - мокирование зависимостей
- **FluentAssertions** - читаемые утверждения
- **TestContainers** - тестирование с реальной БД

### Покрытие тестами
- Сервисы: 90%+
- ViewModels: 80%+
- Критические пути: 100%