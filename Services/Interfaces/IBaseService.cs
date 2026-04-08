namespace CustomPcStoreApp.Services.Interfaces;

/// <summary>
/// Базовый интерфейс для всех сервисов
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public interface IBaseService<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
}