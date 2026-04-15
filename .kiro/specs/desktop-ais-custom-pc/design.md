# Проектирование десктопного приложения АИС "Кастом ПК"

## Обзор

Десктопное приложение АИС "Кастом ПК" представляет собой WPF-приложение на платформе .NET 8, предназначенное для автоматизации процессов управления продажами и сборкой кастомных ПК. Приложение использует трехуровневую архитектуру с четким разделением ответственности между слоями представления, бизнес-логики и доступа к данным.

## Архитектура

### Общая архитектура системы

Система построена по принципу многослойной архитектуры:

1. **Presentation Layer (Слой представления)** - WPF интерфейс пользователя
2. **Business Logic Layer (Слой бизнес-логики)** - сервисы и бизнес-правила
3. **Data Access Layer (Слой доступа к данным)** - репозитории и Entity Framework
4. **Database Layer (Слой базы данных)** - MS SQL Server база данных

### Технологический стек

- **Frontend**: WPF (.NET 8) с MVVM паттерном
- **Backend**: C# .NET 8
- **ORM**: Entity Framework Core
- **Database**: MS SQL Server (совместимость с MySQL через адаптер)
- **DI Container**: Microsoft.Extensions.DependencyInjection
- **Authentication**: BCrypt для хеширования паролей
- **Logging**: Microsoft.Extensions.Logging

## Компоненты и интерфейсы

### Слой представления (Presentation Layer)

#### Главные окна и представления

1. **LoginWindow** - окно аутентификации
   - Поля ввода логина и пароля
   - Кнопка входа и восстановления пароля
   - Валидация на стороне клиента

2. **MainWindow** - главное окно приложения
   - Меню навигации по модулям
   - Панель статуса пользователя
   - Область контента для загрузки представлений

3. **ClientsView** - управление клиентами
   - Список клиентов с поиском и фильтрацией
   - Формы создания/редактирования клиентов
   - Разделение на физических и юридических лиц

4. **OrdersView** - управление заказами
   - Список заказов с группировкой по статусам
   - Конфигуратор ПК для создания заказов
   - Калькулятор стоимости

5. **AssemblyView** - сборочные задания
   - Канбан-доска заданий по статусам
   - Детальная информация о компонентах
   - Таймер работы сборщика

6. **StockView** - управление складом
   - Остатки компонентов по складам
   - Операции поступления/списания
   - Индикаторы критических остатков

7. **ReportsView** - отчеты и аналитика
   - Фильтры по периодам и параметрам
   - Экспорт в Excel/PDF
   - Графики и диаграммы

#### Контроллеры (Controllers)

Каждое представление имеет соответствующий контроллер, который:
- Обрабатывает пользовательский ввод
- Взаимодействует с сервисами бизнес-логики
- Управляет состоянием представления
- Обеспечивает валидацию данных

### Слой бизнес-логики (Business Logic Layer)

#### Основные сервисы

1. **IAuthService / AuthService**
   ```csharp
   public interface IAuthService
   {
       Task<User> AuthenticateAsync(string login, string password);
       Task<bool> ValidateTokenAsync(string token);
       Task LogoutAsync(int userId);
       Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
   }
   ```

2. **IOrderService / OrderService**
   ```csharp
   public interface IOrderService
   {
       Task<Order> CreateOrderAsync(int clientId, List<OrderComponent> components);
       Task<Order> GetOrderByIdAsync(int orderId);
       Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
       Task<decimal> CalculateOrderTotalAsync(List<OrderComponent> components);
       Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status);
   }
   ```

3. **IAssemblyService / AssemblyService**
   ```csharp
   public interface IAssemblyService
   {
       Task<AssemblyTask> CreateAssemblyTaskAsync(int orderId, string configurationName);
       Task<bool> AssignEmployeeAsync(int assemblyId, int employeeId);
       Task<bool> StartAssemblyAsync(int assemblyId);
       Task<bool> CompleteAssemblyAsync(int assemblyId);
       Task<List<AssemblyTask>> GetTasksByEmployeeAsync(int employeeId);
   }
   ```

4. **IStockService / StockService**
   ```csharp
   public interface IStockService
   {
       Task<int> GetStockQuantityAsync(int componentId, int warehouseId);
       Task<bool> UpdateStockAsync(int componentId, int warehouseId, int quantity, string operation);
       Task<bool> ReserveComponentsAsync(int assemblyId);
       Task<List<StockBalance>> GetLowStockItemsAsync(int threshold = 10);
   }
   ```

#### Бизнес-правила

- **Валидация переходов статусов заказов**: новый → в_работе → собран → отгружен → выполнен
- **Проверка наличия компонентов** перед созданием заказа
- **Автоматическое создание сборочного задания** при переводе заказа в статус "в_работе"
- **Списание компонентов со склада** при завершении сборки
- **Расчет стоимости заказа** на основе текущих цен компонентов

### Слой доступа к данным (Data Access Layer)

#### Entity Framework модели

```csharp
public class User
{
    public int UserId { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public int RoleId { get; set; }
    public int? EmployeeId { get; set; }
    public int? ClientId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
    
    public Role Role { get; set; }
    public Employee Employee { get; set; }
    public Client Client { get; set; }
}

public class Order
{
    public int OrderId { get; set; }
    public int ClientId { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    
    public Client Client { get; set; }
    public List<AssemblyTask> AssemblyTasks { get; set; }
}
```

#### Репозитории

Каждая сущность имеет соответствующий репозиторий с базовыми CRUD операциями:

```csharp
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
}

public interface IOrderRepository : IRepository<Order>
{
    Task<List<Order>> GetOrdersByClientAsync(int clientId);
    Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status);
    Task<List<Order>> GetOrdersByDateRangeAsync(DateTime from, DateTime to);
}
```

## Модели данных

### Основные сущности

1. **User (Пользователь)**
   - Аутентификация и авторизация
   - Связь с сотрудником или клиентом
   - Роли и права доступа

2. **Client (Клиент)**
   - Физические и юридические лица
   - Контактная информация
   - История заказов

3. **Order (Заказ)**
   - Конфигурация ПК
   - Статусы выполнения
   - Стоимость и даты

4. **AssemblyTask (Сборочное задание)**
   - Назначение исполнителя
   - Компоненты для сборки
   - Временные метки

5. **Component (Компонент)**
   - Каталог компонентов ПК
   - Типы и производители
   - Складские остатки

### Перечисления

```csharp
public enum OrderStatus
{
    New = 1,
    InProgress = 2,
    Assembled = 3,
    Shipped = 4,
    Completed = 5
}

public enum AssemblyStatus
{
    Waiting = 1,
    InAssembly = 2,
    Ready = 3
}

public enum ClientType
{
    Individual = 1,
    LegalEntity = 2
}

public enum UserRole
{
    Administrator = 1,
    Manager = 2,
    Assembler = 3,
    Client = 4
}
```

## Обработка ошибок

### Стратегия обработки ошибок

1. **Валидация на уровне UI** - немедленная обратная связь пользователю
2. **Бизнес-валидация в сервисах** - проверка бизнес-правил
3. **Обработка исключений базы данных** - логирование и пользовательские сообщения
4. **Глобальный обработчик исключений** - для непредвиденных ошибок

### Типы исключений

```csharp
public class BusinessLogicException : Exception
{
    public string UserMessage { get; }
    public BusinessLogicException(string userMessage, string technicalMessage) 
        : base(technicalMessage)
    {
        UserMessage = userMessage;
    }
}

public class ValidationException : Exception
{
    public Dictionary<string, string> ValidationErrors { get; }
    public ValidationException(Dictionary<string, string> errors) 
        : base("Validation failed")
    {
        ValidationErrors = errors;
    }
}
```

## Стратегия тестирования

### Уровни тестирования

1. **Unit Tests** - тестирование сервисов и бизнес-логики
   - Использование Moq для мокирования зависимостей
   - Покрытие всех бизнес-правил
   - Тестирование валидации данных

2. **Integration Tests** - тестирование взаимодействия с базой данных
   - In-Memory база данных для тестов
   - Тестирование репозиториев
   - Проверка миграций

3. **UI Tests** - автоматизированное тестирование интерфейса
   - Использование WPF Test Framework
   - Тестирование основных пользовательских сценариев
   - Проверка валидации форм

### Тестовые данные

Создание набора тестовых данных для:
- Различных ролей пользователей
- Клиентов разных типов
- Заказов в различных статусах
- Компонентов и складских остатков

## Дизайн пользовательского интерфейса

### Цветовая схема (серые тона)

```xml
<ResourceDictionary>
    <!-- Основные цвета -->
    <SolidColorBrush x:Key="PrimaryGray" Color="#FF2D2D30"/>
    <SolidColorBrush x:Key="SecondaryGray" Color="#FF3F3F46"/>
    <SolidColorBrush x:Key="LightGray" Color="#FF68217A"/>
    <SolidColorBrush x:Key="BackgroundGray" Color="#FFF0F0F0"/>
    <SolidColorBrush x:Key="TextGray" Color="#FF1E1E1E"/>
    
    <!-- Акцентные цвета -->
    <SolidColorBrush x:Key="AccentBlue" Color="#FF007ACC"/>
    <SolidColorBrush x:Key="SuccessGreen" Color="#FF16C60C"/>
    <SolidColorBrush x:Key="WarningOrange" Color="#FFFF8C00"/>
    <SolidColorBrush x:Key="ErrorRed" Color="#FFE74C3C"/>
</ResourceDictionary>
```

### Принципы дизайна

1. **Минимализм** - чистый интерфейс без лишних элементов
2. **Консистентность** - единообразие элементов управления
3. **Доступность** - поддержка сенсорного ввода и клавиатуры
4. **Отзывчивость** - индикаторы загрузки и обратная связь
5. **Иерархия** - четкое разделение информации по важности

### Компоненты интерфейса

1. **Заглушки изображений** - серые прямоугольники с иконками
2. **Индикаторы статусов** - цветовое кодирование
3. **Формы ввода** - валидация в реальном времени
4. **Таблицы данных** - сортировка и фильтрация
5. **Диалоговые окна** - подтверждение операций

## Безопасность

### Аутентификация и авторизация

1. **Хеширование паролей** - BCrypt с солью
2. **Сессии пользователей** - токены с ограниченным временем жизни
3. **Разграничение доступа** - проверка ролей на уровне UI и сервисов
4. **Аудит действий** - логирование всех операций изменения данных

### Защита данных

1. **Валидация входных данных** - предотвращение SQL-инъекций
2. **Шифрование соединения** - SSL/TLS для подключения к БД
3. **Резервное копирование** - автоматические бэкапы базы данных
4. **Логирование безопасности** - отслеживание попыток несанкционированного доступа

## Производительность

### Оптимизация базы данных

1. **Индексы** - на часто используемые поля для поиска
2. **Пагинация** - загрузка данных порциями
3. **Кеширование** - часто запрашиваемые справочники
4. **Ленивая загрузка** - связанные данные по требованию

### Оптимизация интерфейса

1. **Виртуализация списков** - для больших объемов данных
2. **Асинхронные операции** - предотвращение блокировки UI
3. **Сжатие изображений** - оптимизация размера файлов
4. **Кеширование представлений** - переиспользование созданных элементов