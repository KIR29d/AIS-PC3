namespace CustomPCManager.Repositories
{
    /// <summary>
    /// Интерфейс репозитория для работы с клиентами
    /// </summary>
    public interface IClientRepository
    {
        Task<Client?> GetByIdAsync(int id);
        Task<IEnumerable<Client>> GetAllAsync();
        Task<int> CreateAsync(Client client);
        Task UpdateAsync(Client client);
        Task DeleteAsync(int id);
        Task<IEnumerable<Client>> SearchAsync(string searchTerm);
    }
}
