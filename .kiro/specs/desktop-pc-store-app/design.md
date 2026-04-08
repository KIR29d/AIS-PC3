# Проектный документ десктопного приложения магазина кастомных ПК

## Обзор

Система представляет собой десктопное приложение на C# (.NET 8) с архитектурой Model-View-ViewModel (MVVM), использующее WPF для пользовательского интерфейса и Entity Framework Core для работы с базой данных MySQL. Приложение обеспечивает автоматизацию процессов управления заказами, складскими остатками и сборочными заданиями в магазине кастомных ПК.

## Архитектура

### Общая архитектура системы

Система построена по трехуровневой архитектуре:

1. **Уровень представления (Presentation Layer)** - WPF Views и ViewModels
2. **Уровень бизнес-логики (Business Layer)** - Services и Business Models
3. **Уровень данных (Data Layer)** - Entity Framework Core, Repositories

### Архитектурные паттерны

- **MVVM (Model-View-ViewModel)** - для разделения логики представления и бизнес-логики
- **Repository Pattern** - для абстракции доступа к данным
- **Unit of Work** - для управления транзакциями
- **Dependency Injection** - для инверсии зависимостей
- **Command Pattern** - для обработки пользовательских действий

## Компоненты и интерфейсы

### Основные модули приложения

#### 1. Модуль аутентификации (Authentication Module)
```
AuthenticationService
├── IAuthenticationService
├── LoginViewModel
├── LoginView
└── UserSession (Singleton)
```

#### 2. Модуль управления компонентами (Components Module)
```
ComponentsModule
├── IComponentService
├── ComponentsViewModel
├── ComponentsView
├── ComponentEditViewModel
└── ComponentEditView
```

#### 3. Модуль управления складом (Warehouse Module)
```
WarehouseModule
├── IWarehouseService
├── WarehouseViewModel
├── WarehouseView
├── StockEditViewModel
└── StockEditView
```

#### 4. Модуль управления клиентами (Clients Module)
```
ClientsModule
├── IClientService
├── ClientsViewModel
├── ClientsView
├── ClientEditViewModel
└── ClientEditView
```

#### 5. Модуль управления заказами (Orders Module)
```
OrdersModule
├── IOrderService
├── OrdersViewModel
├── OrdersView
├── OrderEditViewModel
└── OrderEditView
```

#### 6. Модуль сборочных заданий (Assembly Module)
```
AssemblyModule
├── IAssemblyService
├── AssemblyTasksViewModel
├── AssemblyTasksView
├── TaskEditViewModel
└── TaskEditView
```

#### 7. Административный модуль (Admin Module)
```
AdminModule
├── IUserManagementService
├── IAuditService
├── UsersViewModel
├── UsersView
├── AuditViewModel
└── AuditView
```

### Основные интерфейсы

```csharp
// Базовый интерфейс для всех сервисов
public interface IBaseService<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
}

// Интерфейс аутентификации
public interface IAuthenticationService
{
    Task<AuthResult> LoginAsync(string username, string password);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<UserInfo> GetCurrentUserAsync();
}

// Интерфейс для работы с заказами
public interface IOrderService : IBaseService<Order>
{
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
    Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
    Task<decimal> CalculateOrderTotalAsync(int orderId);
}
```

## Модели данных

### Основные Entity модели

```csharp
// Базовая модель
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Модель пользователя
public class User : BaseEntity
{
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public int? EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public int? ClientId { get; set; }
    public Client Client { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastLoginDate { get; set; }
}

// Модель клиента
public class Client : BaseEntity
{
    public ClientType ClientType { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string OrganizationName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime RegistrationDate { get; set; }
    public ICollection<Order> Orders { get; set; }
}

// Модель заказа
public class Order : BaseEntity
{
    public int ClientId { get; set; }
    public Client Client { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ShippingDate { get; set; }
    public ICollection<AssemblyTask> AssemblyTasks { get; set; }
}
```

### ViewModel модели

```csharp
// Базовая ViewModel
public abstract class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

// ViewModel для главного окна
public class MainWindowViewModel : BaseViewModel
{
    public ICommand NavigateCommand { get; }
    public ObservableCollection<MenuItem> MenuItems { get; }
    public BaseViewModel CurrentViewModel { get; set; }
    public UserInfo CurrentUser { get; set; }
}

// ViewModel для управления заказами
public class OrdersViewModel : BaseViewModel
{
    public ObservableCollection<OrderViewModel> Orders { get; }
    public OrderViewModel SelectedOrder { get; set; }
    public ICommand CreateOrderCommand { get; }
    public ICommand EditOrderCommand { get; }
    public ICommand DeleteOrderCommand { get; }
    public ICommand RefreshCommand { get; }
}
```

## Дизайн пользовательского интерфейса

### Цветовая схема (серые тона)

```xml
<!-- Основная цветовая палитра -->
<Color x:Key="PrimaryGray">#FF2D2D30</Color>
<Color x:Key="SecondaryGray">#FF3F3F46</Color>
<Color x:Key="LightGray">#FFE5E5E5</Color>
<Color x:Key="MediumGray">#FF9E9E9E</Color>
<Color x:Key="DarkGray">#FF424242</Color>
<Color x:Key="AccentGray">#FF616161</Color>

<!-- Статусные цвета -->
<Color x:Key="SuccessGray">#FF757575</Color>
<Color x:Key="WarningGray">#FF9E9E9E</Color>
<Color x:Key="ErrorGray">#FF424242</Color>
```

### Структура главного окна

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/> <!-- Заголовок -->
        <RowDefinition Height="Auto"/> <!-- Меню -->
        <RowDefinition Height="*"/>    <!-- Основной контент -->
        <RowDefinition Height="Auto"/> <!-- Статусная строка -->
    </Grid.RowDefinitions>
    
    <!-- Заголовок приложения -->
    <Border Grid.Row="0" Background="{StaticResource PrimaryGray}">
        <TextBlock Text="АИС Кастом ПК" Style="{StaticResource HeaderStyle}"/>
    </Border>
    
    <!-- Главное меню -->
    <Menu Grid.Row="1" Background="{StaticResource SecondaryGray}">
        <!-- Пункты меню в зависимости от роли пользователя -->
    </Menu>
    
    <!-- Область контента -->
    <ContentPresenter Grid.Row="2" Content="{Binding CurrentViewModel}"/>
    
    <!-- Статусная строка -->
    <StatusBar Grid.Row="3" Background="{StaticResource LightGray}">
        <TextBlock Text="{Binding StatusMessage}"/>
    </StatusBar>
</Grid>
```

### Макеты основных форм

#### Форма входа в систему
- Центрированная форма с полями логин/пароль
- Кнопка "Войти" и ссылка "Забыли пароль?"
- Логотип-заглушка (серый прямоугольник)

#### Список заказов
- DataGrid с колонками: ID, Клиент, Статус, Сумма, Дата
- Панель инструментов: Создать, Редактировать, Удалить, Обновить
- Фильтры по статусу и дате

#### Форма редактирования заказа
- Поля для выбора клиента
- Таблица компонентов с возможностью добавления/удаления
- Автоматический расчет общей суммы
- Кнопки Сохранить/Отмена

## Обработка ошибок

### Стратегия обработки ошибок

1. **Глобальный обработчик исключений** - перехват необработанных исключений
2. **Логирование** - запись всех ошибок в файл и базу данных
3. **Пользовательские уведомления** - дружественные сообщения об ошибках
4. **Валидация данных** - проверка на уровне ViewModel и Entity

### Типы ошибок

```csharp
public class BusinessException : Exception
{
    public string UserMessage { get; }
    public BusinessException(string userMessage, string technicalMessage) 
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

1. **Unit Tests** - тестирование отдельных компонентов
   - Сервисы бизнес-логики
   - ViewModels
   - Валидаторы

2. **Integration Tests** - тестирование интеграции
   - Работа с базой данных
   - API взаимодействие

3. **UI Tests** - тестирование пользовательского интерфейса
   - Основные пользовательские сценарии
   - Навигация между формами

### Инструменты тестирования

- **xUnit** - основной фреймворк для unit тестов
- **Moq** - создание mock объектов
- **FluentAssertions** - более читаемые assertions
- **TestContainers** - тестирование с реальной базой данных

### Покрытие тестами

- Минимальное покрытие кода: 80%
- Обязательное покрытие критических бизнес-процессов: 100%
- Тестирование всех публичных API методов

## Безопасность

### Аутентификация и авторизация

1. **Хеширование паролей** - использование BCrypt
2. **Сессии пользователей** - управление активными сессиями
3. **Разграничение доступа** - проверка прав на уровне UI и бизнес-логики

### Аудит действий

```csharp
public class AuditService : IAuditService
{
    public async Task LogActionAsync(string tableName, string operation, 
        int userId, object details)
    {
        var auditRecord = new AuditLog
        {
            TableName = tableName,
            Operation = operation,
            UserId = userId,
            ChangeDate = DateTime.Now,
            Details = JsonSerializer.Serialize(details)
        };
        
        await _repository.CreateAsync(auditRecord);
    }
}
```

## Производительность

### Оптимизация запросов к БД

1. **Lazy Loading** - загрузка связанных данных по требованию
2. **Пагинация** - разбиение больших списков на страницы
3. **Кеширование** - кеширование часто используемых справочников
4. **Индексы** - оптимизация запросов через индексы БД

### Оптимизация UI

1. **Виртуализация** - для больших списков данных
2. **Асинхронные операции** - предотвращение блокировки UI
3. **Прогресс-индикаторы** - информирование о длительных операциях