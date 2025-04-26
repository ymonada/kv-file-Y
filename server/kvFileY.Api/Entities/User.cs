namespace kvFileY.Domain.Entities;

public class User
{
    public int  Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; }  = string.Empty;
    public ICollection<FileY> Files { get; set; } = [];

}