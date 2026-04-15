namespace CustomPCManager.Models
{
    /// <summary>
    /// Модель остатков компонента на складе
    /// </summary>
    public class StockBalance
    {
        public int компонент_id { get; set; }
        public int склад_id { get; set; }
        public int количество { get; set; }
        public DateTime дата_обновления { get; set; }

        // Навигационные свойства
        public Component? Компонент { get; set; }
        public Warehouse? Склад { get; set; }

        public StockBalance()
        {
            дата_обновления = DateTime.Now;
        }

        /// <summary>
        /// Обновление количества
        /// </summary>
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Количество не может быть отрицательным");
            
            количество = newQuantity;
            дата_обновления = DateTime.Now;
        }

        /// <summary>
        /// Добавление количества
        /// </summary>
        public void AddQuantity(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Добавляемое количество не может быть отрицательным");
            
            количество += amount;
            дата_обновления = DateTime.Now;
        }

        /// <summary>
        /// Удаление количества (резервирование или списание)
        /// </summary>
        public bool RemoveQuantity(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Снимаемое количество не может быть отрицательным");
            
            if (количество < amount)
                return false; // Недостаточно на складе
            
            количество -= amount;
            дата_обновления = DateTime.Now;
            return true;
        }
    }
}
