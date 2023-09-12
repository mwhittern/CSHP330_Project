using System.Net;
using System.Text;
using System.Text.Json;

namespace UnitTests;

public class Tests
{
    HttpClient client;
    
    [SetUp]
    public void Setup()
    {
        string userEmail = "admin@email.com";
        string userPassword = "adminadmin";
        
        client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5014/api/");

        var tokenResult = client.GetAsync($"/login/{userEmail}/{userPassword}").Result;
        var tokenJson = tokenResult.Content.ReadAsStringAsync().Result;
        var token = JsonSerializer.Deserialize<Token>(tokenJson);

        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.TokenString);
    }

    [Test]
    public async Task GetSpecificGood()
    {
        var result = await client.GetAsync($"api/User/101");
        
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
    
    [Test]
    public async Task GetSpecificBad()
    {
        var result = await client.GetAsync($"api/User/999999");
        
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
    
    [Test]
    public async Task AddInvalidUser()
    {
        var newUser = new User(1, null, "123");

        var asJson = JsonSerializer.Serialize(newUser);
        var postContent = new StringContent(asJson, Encoding.UTF8, "application/json");
        var result = await client.PostAsync("User", postContent);
        
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    
    [Test]
    public async Task AddValidUser()
    {
        var newUser = new User(1, "test@test.com", "123");

        var asJson = JsonSerializer.Serialize(newUser);
        var postContent = new StringContent(asJson, Encoding.UTF8, "application/json");
        var result = await client.PostAsync("User", postContent);
        
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }
    
    [Test]
    public async Task DeleteValidUser()
    {
        var result = await client.DeleteAsync("User/103");
        
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test]
    public async Task DeleteInvalidUser()
    {
        var result = await client.DeleteAsync("User/8888");
        
        Assert.That( result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
    
    [Test]
    public async Task UpdateExistingUser()
    {
        var newUser = new User(101, "NewEmail@test.com", "123");

        var asJson = JsonSerializer.Serialize(newUser);
        var postContent = new StringContent(asJson, Encoding.UTF8, "application/json");
        var result = await client.PutAsync("User", postContent);
        
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test]
    public async Task UpdateUnknownUser()
    {
        var newUser = new User(1, "test@test.com", "88888");

        var asJson = JsonSerializer.Serialize(newUser);
        var postContent = new StringContent(asJson, Encoding.UTF8, "application/json");
        var result = await client.PutAsync("User", postContent);
        
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}