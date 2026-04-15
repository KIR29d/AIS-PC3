namespace CustomPCManager.Models
{
    /// <summary>
    /// Модель склада
    /// </summary>
    public class Warehouse
    {
        public int склад_id { get; set; }
        public string название { get; set; } = string.Empty;
        public string адрес { get; set; } = string.Empty;

        public Warehouse()
        {
        }

        public Warehouse(int id, string name, string address)
        {
            склад_id = id;
            название = name;
            адрес = address;
        }

        /// <summary>
        /// Обновление адреса склада
        /// </summary>
        public void UpdateAddress(string newAddress)
        {
            адрес = newAddress;
        }
    }
}
