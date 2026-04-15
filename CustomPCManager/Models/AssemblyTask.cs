namespace CustomPCManager.Models
{
    /// <summary>
    /// Модель сборочного задания
    /// </summary>
    public class AssemblyTask
    {
        // Допустимые статусы сборки
        public static class Statuses
        {
            public const string Ожидает = "ожидает";
            public const string ВСборке = "в_сборке";
            public const string Готово = "готово";
        }

        public int сборка_id { get; set; }
        public string название_конфигурации { get; set; } = string.Empty;
        public string статус { get; set; } = Statuses.Ожидает;
        public int? сотрудник_id { get; set; }
        public int? заказ_id { get; set; }
        public DateTime? дата_начала { get; set; }
        public DateTime? дата_завершения { get; set; }

        // Навигационные свойства
        public Employee? Сотрудник { get; set; }
        public Order? Заказ { get; set; }
        public List<AssemblyComponent>? Компоненты { get; set; }

        public AssemblyTask()
        {
            статус = Statuses.Ожидает;
        }

        /// <summary>
        /// Начало сборки с назначением сотрудника
        /// </summary>
        public void StartAssembly(int employeeId)
        {
            if (статус != Statuses.Ожидает)
                throw new InvalidOperationException("Сборка уже началась или завершена");
            
            сотрудник_id = employeeId;
            статус = Statuses.ВСборке;
            дата_начала = DateTime.Now;
        }

        /// <summary>
        /// Завершение сборки
        /// </summary>
        public void CompleteAssembly()
        {
            if (статус != Statuses.ВСборке)
                throw new InvalidOperationException("Сборка не находится в процессе");
            
            статус = Statuses.Готово;
            дата_завершения = DateTime.Now;
        }

        /// <summary>
        /// Добавление компонента в сборку
        /// </summary>
        public void AddComponent(int componentId, int quantity)
        {
            if (Компоненты == null)
                Компоненты = new List<AssemblyComponent>();
            
            var existing = Компоненты.FirstOrDefault(c => c.компонент_id == componentId);
            if (existing != null)
            {
                existing.количество += quantity;
            }
            else
            {
                Компоненты.Add(new AssemblyComponent
                {
                    сборка_id = сборка_id,
                    компонент_id = componentId,
                    количество = quantity
                });
            }
        }

        /// <summary>
        /// Получить список компонентов
        /// </summary>
        public List<AssemblyComponent> GetComponents()
        {
            return Компоненты ?? new List<AssemblyComponent>();
        }
    }
}
