namespace CustomPCManager.Models
{
    /// <summary>
    /// Модель компонента ПК
    /// </summary>
    public class Component
    {
        public int компонент_id { get; set; }
        public string наименование { get; set; } = string.Empty;
        public string тип { get; set; } = string.Empty; // CPU, GPU, RAM, SSD, HDD, PSU, Case, etc.
        public string производитель { get; set; } = string.Empty;

        public Component()
        {
        }

        public Component(string name, string type, string manufacturer)
        {
            наименование = name;
            тип = type;
            производитель = manufacturer;
        }
    }
}
