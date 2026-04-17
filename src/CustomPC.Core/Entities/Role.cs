namespace CustomPC.Core.Entities;

/// <summary>
/// Роль пользователя в системе
/// </summary>
public class Role
{
    public int роль_id { get; set; }
    public string название_роли { get; set; } = string.Empty;

    public Role()
    {
    }

    public Role(string название_роли)
    {
        this.название_роли = название_роли;
    }

    public bool Validate()
    {
        return !string.IsNullOrWhiteSpace(название_роли);
    }
}
