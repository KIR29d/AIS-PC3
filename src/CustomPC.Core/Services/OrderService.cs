using CustomPC.Core.Entities;
using CustomPC.Core.Interfaces;

namespace CustomPC.Core.Services;

/// <summary>
/// Сервис управления заказами
/// </summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IAssemblyRepository _assemblyRepository;

    public OrderService(
        IOrderRepository orderRepository,
        IClientRepository clientRepository,
        IAssemblyRepository assemblyRepository)
    {
        _orderRepository = orderRepository;
        _clientRepository = clientRepository;
        _assemblyRepository = assemblyRepository;
    }

    public async Task<Order> CreateOrderAsync(int clientId)
    {
        // Проверка существования клиента
        var client = await _clientRepository.GetByIdAsync(clientId);
        if (client == null)
        {
            throw new InvalidOperationException($"Клиент с ID {clientId} не найден");
        }

        var order = new Order
        {
            клиент_id = clientId,
            статус = "новый",
            общая_сумма = 0,
            дата_создания = DateTime.UtcNow
        };

        return await _orderRepository.CreateAsync(order);
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _orderRepository.GetByIdAsync(id);
    }

    public async Task UpdateStatusAsync(int orderId, string status)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            throw new InvalidOperationException($"Заказ с ID {orderId} не найден");
        }

        // Валидация перехода статуса
        ValidateStatusTransition(order.статус, status);

        order.ChangeStatus(status);
        await _orderRepository.UpdateAsync(order);
    }

    public async Task<decimal> CalculateTotalAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            throw new InvalidOperationException($"Заказ с ID {orderId} не найден");
        }

        // Здесь должна быть логика расчета общей суммы
        // на основе компонентов в сборочных заданиях
        // Пока возвращаем текущую сумму
        return order.общая_сумма;
    }

    public async Task<IEnumerable<Order>> GetOrdersByClientIdAsync(int clientId)
    {
        return await _orderRepository.GetByClientIdAsync(clientId);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    /// <summary>
    /// Валидация перехода между статусами заказа
    /// </summary>
    private void ValidateStatusTransition(string currentStatus, string newStatus)
    {
        var validTransitions = new Dictionary<string, List<string>>
        {
            ["новый"] = new List<string> { "в_работе" },
            ["в_работе"] = new List<string> { "собран" },
            ["собран"] = new List<string> { "отгружен" },
            ["отгружен"] = new List<string> { "выполнен" }
        };

        if (currentStatus == newStatus)
        {
            return; // Разрешаем установку того же статуса
        }

        if (!validTransitions.ContainsKey(currentStatus) ||
            !validTransitions[currentStatus].Contains(newStatus))
        {
            throw new InvalidOperationException(
                $"Недопустимый переход статуса: '{currentStatus}' -> '{newStatus}'");
        }
    }
}
