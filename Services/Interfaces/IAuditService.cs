using System.Threading.Tasks;

namespace PCStoreApp.Services.Interfaces
{
    public interface IAuditService
    {
        Task LogChangeAsync(string tableName, string operation, int userId, string details);
        Task LogChangeAsync(string tableName, string operation, string details);
    }
}