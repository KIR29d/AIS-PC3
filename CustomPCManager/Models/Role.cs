namespace CustomPCManager.Models
{
    /// <summary>
    /// Модель роли пользователя
    /// </summary>
    public class Role
    {
        public int роль_id { get; set; }
        public string название_роли { get; set; } = string.Empty;

        public Role()
        {
        }

        public Role(int id, string name)
        {
            роль_id = id;
            название_роли = name;
        }

        public bool Validate()
        {
            return !string.IsNullOrWhiteSpace(название_роли);
        }
    }
}
