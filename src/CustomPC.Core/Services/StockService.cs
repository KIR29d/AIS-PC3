using CustomPC.Core.Entities;
using CustomPC.Core.Interfaces;
using CustomPC.Infrastructure.Repositories;

namespace CustomPC.Core.Services;

/// <summary>
/// Сервис управления складом
/// </summary>
public class StockService : IStockService
{
    private readonly IStockRepository _stockRepository;

    public StockService(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task<int> GetStockQuantityAsync(int componentId, int warehouseId)
    {
        var stock = await _stockRepository.GetAsync(componentId, warehouseId);
        return stock?.количество ?? 0;
    }

    public async Task UpdateStockAsync(int componentId, int warehouseId, int quantity)
    {
        var stock = await _stockRepository.GetAsync(componentId, warehouseId);
        
        if (stock == null)
        {
            // Создаем новую запись об остатке
            stock = new StockBalance
            {
                компонент_id = componentId,
                склад_id = warehouseId,
                количество = quantity
            };
            await _stockRepository.CreateAsync(stock);
        }
        else
        {
            stock.UpdateQuantity(quantity);
            await _stockRepository.UpdateAsync(stock);
        }
    }

    public async Task<bool> ReserveComponentsAsync(int assemblyId)
    {
        // Логика резервирования компонентов для сборочного задания
        // В реальной реализации нужно получать компоненты из сборки
        // и уменьшать их количество на складе
        return await Task.FromResult(true);
    }

    public async Task<bool> HasSufficientStockAsync(int componentId, int warehouseId, int quantity)
    {
        var currentStock = await GetStockQuantityAsync(componentId, warehouseId);
        return currentStock >= quantity;
    }

    public async Task<IEnumerable<StockBalance>> GetComponentStockAsync(int componentId)
    {
        return await _stockRepository.GetByComponentIdAsync(componentId);
    }
}
