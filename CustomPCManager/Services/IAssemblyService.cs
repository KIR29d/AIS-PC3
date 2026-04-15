using CustomPCManager.Models;

namespace CustomPCManager.Services
{
    /// <summary>
    /// Интерфейс сервиса для работы со сборочными заданиями
    /// </summary>
    public interface IAssemblyService
    {
        Task<AssemblyTask?> GetByIdAsync(int id);
        Task<AssemblyTask> CreateAssemblyTaskAsync(string name, int? orderId, int userId);
        Task AssignEmployeeAsync(int assemblyId, int employeeId, int userId);
        Task StartAssemblyAsync(int assemblyId, int employeeId, int userId);
        Task CompleteAssemblyAsync(int assemblyId, int userId);
        Task AddComponentAsync(int assemblyId, int componentId, int quantity, int userId);
        Task<IEnumerable<AssemblyTask>> GetByStatusAsync(string status);
    }
}
