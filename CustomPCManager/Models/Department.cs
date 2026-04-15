namespace CustomPCManager.Models
{
    /// <summary>
    /// Модель отдела
    /// </summary>
    public class Department
    {
        public int отдел_id { get; set; }
        public string название { get; set; } = string.Empty;

        public Department()
        {
        }

        public Department(int id, string name)
        {
            отдел_id = id;
            название = name;
        }
    }
}
