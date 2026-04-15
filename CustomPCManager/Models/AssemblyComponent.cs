namespace CustomPCManager.Models
{
    /// <summary>
    /// Модель связи компонента со сборочным заданием
    /// </summary>
    public class AssemblyComponent
    {
        public int сборка_id { get; set; }
        public int компонент_id { get; set; }
        public int количество { get; set; }

        // Навигационные свойства
        public AssemblyTask? СборочноеЗадание { get; set; }
        public Component? Компонент { get; set; }

        public AssemblyComponent()
        {
        }

        public AssemblyComponent(int assemblyId, int componentId, int quantity)
        {
            сборка_id = assemblyId;
            компонент_id = componentId;
            количество = quantity;
        }

        /// <summary>
        /// Проверка корректности количества
        /// </summary>
        public bool ValidateQuantity()
        {
            return количество > 0;
        }
    }
}
