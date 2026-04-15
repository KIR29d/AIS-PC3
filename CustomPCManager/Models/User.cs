namespace CustomPCManager.Models
{
    /// <summary>
    /// Модель пользователя системы
    /// </summary>
    public class User
    {
        public int пользователь_id { get; set; }
        public string логин { get; set; } = string.Empty;
        public string пароль { get; set; } = string.Empty; // Должен храниться в хэшированном виде
        public int роль_id { get; set; }
        public int? сотрудник_id { get; set; }
        public int? клиент_id { get; set; }
        public bool активен { get; set; } = true;
        public DateTime дата_создания { get; set; }
        public DateTime? дата_последнего_входа { get; set; }

        // Навигационные свойства
        public Role? Роль { get; set; }
        public Employee? Сотрудник { get; set; }
        public Client? Клиент { get; set; }

        public User()
        {
            дата_создания = DateTime.Now;
            активен = true;
        }

        /// <summary>
        /// Проверка пароля (в реальном приложении должно быть сравнение хэшей)
        /// </summary>
        public bool Authenticate(string password)
        {
            // TODO: Реализовать проверку хэша пароля
            return пароль == password && активен;
        }

        /// <summary>
        /// Обновление даты последнего входа
        /// </summary>
        public void UpdateLastLogin()
        {
            дата_последнего_входа = DateTime.Now;
        }

        /// <summary>
        /// Проверка активности пользователя
        /// </summary>
        public bool IsActive() => активен;
    }
}
