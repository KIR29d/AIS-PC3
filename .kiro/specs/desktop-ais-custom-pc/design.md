# Дизайн десктопного приложения АИС "Кастом ПК"

## Обзор

Десктопное приложение АИС "Кастом ПК" будет разработано на платформе .NET 8 с использованием WPF для создания современного пользовательского интерфейса. Приложение использует архитектурный паттерн MVVM (Model-View-ViewModel) для обеспечения разделения логики и представления. Подключение к базе данных MySQL осуществляется через Entity Framework Core с использованием NuGet пакета MySql.EntityFrameworkCore.

## Архитектура

### Архитектурные слои

1. **Presentation Layer (WPF Views + ViewModels)**
   - MainWindow - главное окно с навигацией
   - LoginWindow - окно авторизации
   - AdminModule - модуль администрирования
   - ManagerModule - модуль менеджера
   - AssemblerModule - модуль сборщика
   - ClientModule - модуль клиента

2. **Business Logic Layer (Services)**
   - AuthenticationService - сервис аутентификации
   - UserService - управление пользователями
   - OrderService - управление заказами
   - ComponentService - управление компонентами
   - AssemblyService - управление сборочными заданиями
   - AuditService - ведение журнала аудита

3. **Data Access Layer (Repositories + DbContext)**
   - CustomPcDbContext - контекст Entity Framework
   - Repository pattern для каждой сущности
   - Unit of Work pattern для транзакций

4. **Domain Layer (Models)**
   - Модели данных, соответствующие структуре БД
   - Бизнес-логика валидации

### Технологический стек

- **.NET 8** - основная платформа
- **WPF** - пользовательский интерфейс
- **Entity Framework Core** - ORM для работы с БД
- **MySql.EntityFrameworkCore** - провайдер MySQL
- **BCrypt.Net-Next** - хеширование паролей
- **Microsoft.Extensions.DependencyInjection** - контейнер зависимостей
- **CommunityToolkit.Mvvm** - MVVM фреймворк

## Компоненты и интерфейсы

### Основные модели данных

```csharp
// Основные entity классы
public class User
{
    public int UserId { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
    // ... другие свойства
}

public class Order
{
    public int OrderId { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; }
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }
    // ... другие свойства
}

public class Component
{
    public int ComponentId { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Manufacturer { get; set; }
    // ... другие свойства
}
```

### Интерфейсы сервисов

```csharp
public interface IAuthenticationService
{
    Task<User> AuthenticateAsync(string login, string password);
    Task<bool> ValidatePermissionAsync(int userId, string permission);
}

public interface IOrderService
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order> CreateOrderAsync(Order order);
    Task UpdateOrderStatusAsync(int orderId, string status);
}

public interface IComponentService
{
    Task<IEnumerable<Component>> GetComponentsAsync();
    Task<IEnumerable<Component>> GetComponentsByTypeAsync(string type);
    Task<bool> CheckCompatibilityAsync(IEnumerable<int> componentIds);
}
```

### Структура пользовательского интерфейса

#### Цветовая схема (серые тона)
- **Основной фон**: #F5F5F5 (светло-серый)
- **Панели**: #E0E0E0 (средне-серый)
- **Границы**: #BDBDBD (темно-серый)
- **Текст**: #212121 (почти черный)
- **Акценты**: #757575 (серый)
- **Кнопки**: #9E9E9E (серый) / #616161 (темно-серый при наведении)

#### Главное окно (MainWindow)
- Верхняя панель с информацией о пользователе и кнопкой выхода
- Боковая навигационная панель с модулями (в зависимости от роли)
- Основная рабочая область для отображения выбранного модуля
- Статусная строка внизу

#### Модули интерфейса

**1. Модуль авторизации**
- Поля ввода логина и пароля
- Кнопка "Войти"
- Заглушка для логотипа компании

**2. Административный модуль**
- Управление пользователями (список, добавление, редактирование)
- Управление ролями и правами
- Справочники (отделы, склады, компоненты)
- Просмотр журнала аудита

**3. Модуль менеджера**
- Список заказов с фильтрацией по статусам
- Детальная информация о заказе
- Создание и назначение сборочных заданий
- Управление статусами заказов

**4. Модуль сборщика**
- Список назначенных заданий
- Детали сборочного задания с компонентами
- Кнопки "Начать сборку" / "Завершить сборку"
- Заглушки для изображений компонентов

**5. Модуль клиента**
- Каталог компонентов по категориям
- Конфигуратор ПК с проверкой совместимости
- Корзина и оформление заказа
- История заказов

## Модели данных

### Entity Framework модели

Модели данных будут точно соответствовать структуре базы данных:

- **Роли** (роли) - справочник ролей пользователей
- **Отделы** (отделы) - справочник отделов
- **Склады** (склады) - справочник складов
- **Компоненты** (компоненты) - каталог компонентов ПК
- **Клиенты** (клиенты) - информация о клиентах
- **Сотрудники** (сотрудники) - информация о сотрудниках
- **Пользователи** (пользователи) - учетные записи системы
- **Заказы** (заказы) - заказы клиентов
- **СборочныеЗадания** (сборочные_задания) - задания на сборку
- **КомпонентыВСборке** (компоненты_в_сборке) - состав сборок
- **ОстаткиНаСкладе** (остатки_на_складе) - складские остатки
- **АудитИзменений** (аудит_изменений) - журнал аудита

### Конфигурация Entity Framework

```csharp
public class CustomPcDbContext : DbContext
{
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Component> Components { get; set; }
    // ... другие DbSet

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Конфигурация маппинга на русские названия таблиц
        modelBuilder.Entity<Role>().ToTable("роли");
        modelBuilder.Entity<User>().ToTable("пользователи");
        // ... другие конфигурации
    }
}
```

## Обработка ошибок

### Стратегия обработки ошибок

1. **Уровень данных**: Обработка исключений подключения к БД
2. **Уровень сервисов**: Валидация бизнес-правил и логирование
3. **Уровень представления**: Отображение пользовательских сообщений

### Типы ошибок

- **DatabaseConnectionException** - ошибки подключения к БД
- **ValidationException** - ошибки валидации данных
- **AuthenticationException** - ошибки аутентификации
- **AuthorizationException** - ошибки авторизации
- **BusinessLogicException** - нарушения бизнес-правил

### Механизм уведомлений

```csharp
public interface INotificationService
{
    void ShowError(string message);
    void ShowWarning(string message);
    void ShowInfo(string message);
    void ShowSuccess(string message);
}
```

## Стратегия тестирования

### Типы тестов

1. **Unit Tests**
   - Тестирование сервисов бизнес-логики
   - Тестирование валидации моделей
   - Тестирование утилитарных классов

2. **Integration Tests**
   - Тестирование работы с базой данных
   - Тестирование взаимодействия между слоями

3. **UI Tests**
   - Тестирование основных пользовательских сценариев
   - Тестирование навигации между модулями

### Инструменты тестирования

- **xUnit** - фреймворк для unit-тестов
- **Moq** - библиотека для создания mock-объектов
- **FluentAssertions** - библиотека для читаемых assertions
- **Microsoft.EntityFrameworkCore.InMemory** - in-memory БД для тестов

### Покрытие тестами

- Критическая бизнес-логика: 90%+
- Сервисы: 80%+
- Репозитории: 70%+
- ViewModels: 60%+

## Безопасность

### Аутентификация и авторизация

1. **Хеширование паролей**: BCrypt с солью
2. **Сессии**: Хранение информации о текущем пользователе в памяти
3. **Права доступа**: Проверка прав на уровне сервисов и UI

### Защита данных

1. **Параметризованные запросы**: Защита от SQL-инъекций
2. **Валидация входных данных**: На всех уровнях приложения
3. **Логирование**: Запись всех критических операций

### Аудит

Все операции INSERT, UPDATE, DELETE автоматически записываются в таблицу аудит_изменений с помощью:
- Перехватчиков Entity Framework
- Автоматического определения пользователя
- Сериализации изменений в JSON

## Производительность

### Оптимизация запросов

1. **Lazy Loading**: Отключено по умолчанию
2. **Explicit Loading**: Загрузка связанных данных по требованию
3. **Projection**: Выборка только необходимых полей
4. **Caching**: Кеширование справочных данных

### Пользовательский интерфейс

1. **Виртуализация**: Для больших списков данных
2. **Асинхронные операции**: Все операции с БД асинхронные
3. **Индикаторы загрузки**: Для длительных операций
4. **Пагинация**: Для больших наборов данных