namespace CustomPCManager.Models
{
    /// <summary>
    /// Модель сотрудника
    /// </summary>
    public class Employee
    {
        public int сотрудник_id { get; set; }
        public string фамилия { get; set; } = string.Empty;
        public string имя { get; set; } = string.Empty;
        public string? отчество { get; set; }
        public string должность { get; set; } = string.Empty;
        public DateOnly дата_приёма { get; set; }
        public int отдел_id { get; set; }

        // Навигационное свойство
        public Department? Отдел { get; set; }

        public Employee()
        {
            дата_приёма = DateOnly.FromDateTime(DateTime.Today);
        }

        /// <summary>
        /// Получить полное имя сотрудника
        /// </summary>
        public string GetFullName()
        {
            return $"{фамилия} {имя} {отчество}".Trim();
        }

        /// <summary>
        /// Получить стаж работы в годах
        /// </summary>
        public int YearsEmployed()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var years = today.Year - дата_приёма.Year;
            if (today.DayOfYear < дата_приёма.DayOfYear)
            {
                years--;
            }
            return Math.Max(0, years);
        }
    }
}
