namespace CustomPCManager.Repositories
{
    /// <summary>
    /// Интерфейс репозитория для работы со сборочными заданиями
    /// </summary>
    public interface IAssemblyRepository
    {
        Task<AssemblyTask?> GetByIdAsync(int id);
        Task<IEnumerable<AssemblyTask>> GetAllAsync();
        Task<IEnumerable<AssemblyTask>> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<AssemblyTask>> GetByEmployeeIdAsync(int employeeId);
        Task<int> CreateAsync(AssemblyTask assemblyTask);
        Task UpdateAsync(AssemblyTask assemblyTask);
        Task DeleteAsync(int id);
        Task<IEnumerable<AssemblyTask>> GetByStatusAsync(string status);
    }
}
