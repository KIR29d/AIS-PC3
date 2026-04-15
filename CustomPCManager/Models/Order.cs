namespace CustomPCManager.Models
{
    /// <summary>
    /// Модель заказа клиента
    /// </summary>
    public class Order
    {
        // Допустимые статусы заказа
        public static class Statuses
        {
            public const string Новый = "новый";
            public const string ВРаботе = "в_работе";
            public const string Собран = "собран";
            public const string Отгружен = "отгружен";
            public const string Выполнен = "выполнен";
        }

        public int заказ_id { get; set; }
        public int клиент_id { get; set; }
        public string статус { get; set; } = Statuses.Новый;
        public decimal общая_сумма { get; set; }
        public DateTime дата_создания { get; set; }
        public DateTime? дата_отгрузки { get; set; }

        // Навигационные свойства
        public Client? Клиент { get; set; }
        public List<AssemblyTask>? СборочныеЗадания { get; set; }

        public Order()
        {
            дата_создания = DateTime.Now;
            общая_сумма = 0;
            статус = Statuses.Новый;
        }

        /// <summary>
        /// Пересчет общей суммы заказа
        /// </summary>
        public decimal CalculateTotal()
        {
            // TODO: Реализовать расчет суммы на основе компонентов в сборочных заданиях
            return общая_сумма;
        }

        /// <summary>
        /// Изменение статуса заказа
        /// </summary>
        public void ChangeStatus(string newStatus)
        {
            var validStatuses = new[] { Statuses.Новый, Statuses.ВРаботе, Statuses.Собран, Statuses.Отгружен, Statuses.Выполнен };
            if (!validStatuses.Contains(newStatus))
                throw new ArgumentException($"Недопустимый статус заказа: {newStatus}");
            
            статус = newStatus;
        }

        /// <summary>
        /// Проверка возможности отгрузки
        /// </summary>
        public bool CanShip()
        {
            return статус == Statuses.Собран;
        }

        /// <summary>
        /// Отгрузка заказа
        /// </summary>
        public void Ship()
        {
            if (!CanShip())
                throw new InvalidOperationException("Заказ не готов к отгрузке");
            
            статус = Statuses.Отгружен;
            дата_отгрузки = DateTime.Now;
        }
    }
}
