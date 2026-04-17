using CustomPC.Core.Entities;
using CustomPC.Core.Interfaces;

namespace CustomPC.Core.Services;

/// <summary>
/// Сервис управления сборкой
/// </summary>
public class AssemblyService : IAssemblyService
{
    private readonly IAssemblyRepository _assemblyRepository;
    private readonly IStockService _stockService;
    private readonly IEmployeeRepository _employeeRepository;

    public AssemblyService(
        IAssemblyRepository assemblyRepository,
        IStockService stockService,
        IEmployeeRepository employeeRepository)
    {
        _assemblyRepository = assemblyRepository;
        _stockService = stockService;
        _employeeRepository = employeeRepository;
    }

    public async Task<AssemblyTask> CreateAssemblyTaskAsync(string name, int? orderId = null)
    {
        var assemblyTask = new AssemblyTask
        {
            название_конфигурации = name,
            заказ_id = orderId,
            статус = "ожидает"
        };

        return await _assemblyRepository.CreateAsync(assemblyTask);
    }

    public async Task AssignEmployeeAsync(int assemblyId, int employeeId)
    {
        var assembly = await _assemblyRepository.GetByIdAsync(assemblyId);
        if (assembly == null)
        {
            throw new InvalidOperationException($"Сборочное задание с ID {assemblyId} не найдено");
        }

        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee == null)
        {
            throw new InvalidOperationException($"Сотрудник с ID {employeeId} не найден");
        }

        assembly.сотрудник_id = employeeId;
        await _assemblyRepository.UpdateAsync(assembly);
    }

    public async Task StartAssemblyAsync(int assemblyId, int employeeId)
    {
        var assembly = await _assemblyRepository.GetByIdAsync(assemblyId);
        if (assembly == null)
        {
            throw new InvalidOperationException($"Сборочное задание с ID {assemblyId} не найдено");
        }

        assembly.StartAssembly(employeeId);
        await _assemblyRepository.UpdateAsync(assembly);
    }

    public async Task CompleteAssemblyAsync(int assemblyId)
    {
        var assembly = await _assemblyRepository.GetByIdAsync(assemblyId);
        if (assembly == null)
        {
            throw new InvalidOperationException($"Сборочное задание с ID {assemblyId} не найдено");
        }

        assembly.CompleteAssembly();
        await _assemblyRepository.UpdateAsync(assembly);

        // Если сборка привязана к заказу, обновляем статус заказа
        if (assembly.заказ_id.HasValue)
        {
            // Здесь можно обновить статус заказа на "собран"
        }
    }

    public async Task AddComponentAsync(int assemblyId, int componentId, int quantity)
    {
        var assembly = await _assemblyRepository.GetByIdAsync(assemblyId);
        if (assembly == null)
        {
            throw new InvalidOperationException($"Сборочное задание с ID {assemblyId} не найдено");
        }

        // Проверка наличия компонента на складе
        // Это упрощенная проверка, в реальности нужно проверять конкретный склад
        var hasStock = await _stockService.HasSufficientStockAsync(componentId, 1, quantity);
        if (!hasStock)
        {
            throw new InvalidOperationException(
                $"Недостаточно компонента с ID {componentId} на складе");
        }

        var assemblyComponent = new AssemblyComponent(assemblyId, componentId, quantity);
        
        // Добавляем компонент в коллекцию сборки
        // В реальной реализации нужно будет сохранить через репозиторий AssemblyComponent
        // или обновить сборку
    }

    public async Task<AssemblyTask?> GetAssemblyByIdAsync(int id)
    {
        return await _assemblyRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<AssemblyTask>> GetAssembliesByOrderIdAsync(int orderId)
    {
        return await _assemblyRepository.GetByOrderIdAsync(orderId);
    }
}
