namespace CustomPCManager.Models
{
    /// <summary>
    /// Модель клиента (физическое или юридическое лицо)
    /// </summary>
    public class Client
    {
        public int клиент_id { get; set; }
        public string тип_клиента { get; set; } = string.Empty; // "физическое" или "юридическое"
        public string? фамилия { get; set; }
        public string? имя { get; set; }
        public string? отчество { get; set; }
        public string? наименование_организации { get; set; }
        public string email { get; set; } = string.Empty;
        public string номер_телефона { get; set; } = string.Empty;
        public DateTime дата_регистрации { get; set; }

        public Client()
        {
            дата_регистрации = DateTime.Now;
        }

        /// <summary>
        /// Проверка: является ли клиент физическим лицом
        /// </summary>
        public bool IsIndividual() => тип_клиента == "физическое";

        /// <summary>
        /// Проверка: является ли клиент юридическим лицом
        /// </summary>
        public bool IsLegalEntity() => тип_клиента == "юридическое";

        /// <summary>
        /// Получить полное имя клиента
        /// </summary>
        public string GetFullName()
        {
            if (IsIndividual())
            {
                return $"{фамилия} {имя} {отчество}".Trim();
            }
            else if (IsLegalEntity())
            {
                return наименование_организации ?? string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// Проверка корректности email
        /// </summary>
        public bool ValidateEmail()
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Проверка корректности телефона (базовая)
        /// </summary>
        public bool ValidatePhone()
        {
            if (string.IsNullOrWhiteSpace(номер_телефона)) return false;
            // Удаляем все нецифровые символы
            var digits = new string(номер_телефона.Where(char.IsDigit).ToArray());
            return digits.Length >= 10 && digits.Length <= 15;
        }

        public bool Validate()
        {
            if (!ValidateEmail()) return false;
            if (!ValidatePhone()) return false;
            if (IsIndividual() && string.IsNullOrWhiteSpace(фамилия)) return false;
            if (IsLegalEntity() && string.IsNullOrWhiteSpace(наименование_организации)) return false;
            return true;
        }
    }
}
