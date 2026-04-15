namespace CustomPCManager.Models
{
    /// <summary>
    /// Модель записи аудита изменений
    /// </summary>
    public class AuditLog
    {
        // Допустимые операции
        public static class Operations
        {
            public const string Insert = "INSERT";
            public const string Update = "UPDATE";
            public const string Delete = "DELETE";
        }

        public int аудит_id { get; set; }
        public string таблица { get; set; } = string.Empty;
        public string операция { get; set; } = string.Empty;
        public int пользователь_id { get; set; }
        public DateTime дата_изменения { get; set; }
        public string детали { get; set; } = string.Empty;

        // Навигационное свойство
        public User? Пользователь { get; set; }

        public AuditLog()
        {
            дата_изменения = DateTime.Now;
        }

        /// <summary>
        /// Логирование операции INSERT
        /// </summary>
        public static AuditLog LogInsert(string tableName, int userId, string details)
        {
            return new AuditLog
            {
                таблица = tableName,
                операция = Operations.Insert,
                пользователь_id = userId,
                детали = details,
                дата_изменения = DateTime.Now
            };
        }

        /// <summary>
        /// Логирование операции UPDATE
        /// </summary>
        public static AuditLog LogUpdate(string tableName, int userId, string details)
        {
            return new AuditLog
            {
                таблица = tableName,
                операция = Operations.Update,
                пользователь_id = userId,
                детали = details,
                дата_изменения = DateTime.Now
            };
        }

        /// <summary>
        /// Логирование операции DELETE
        /// </summary>
        public static AuditLog LogDelete(string tableName, int userId, string details)
        {
            return new AuditLog
            {
                таблица = tableName,
                операция = Operations.Delete,
                пользователь_id = userId,
                детали = details,
                дата_изменения = DateTime.Now
            };
        }
    }
}
