using System.Text.Json.Serialization;

namespace UnitTests;

public class User
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("email")]
    public string email { get; set; }
    [JsonPropertyName("password")]
    public string password { get; set; }
    
    [JsonPropertyName("created_date")]
    public DateTime createdDate { get; set; }

    public User(int Id, string email, string password)
    {
        this.Id = Id;
        this.email = email;
        this.password = password;
        this.createdDate = DateTime.UtcNow;
    }
}