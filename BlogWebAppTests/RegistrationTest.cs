using System.Net;
using System.Net.Http.Json;
using BlogWebAppTests.TestModels;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;

namespace BlogWebAppTests;

public class RegistrationTest
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }
    
    [TearDown]
    public void Teardown()
    {
        Helper.TriggerRebuild();
    }
    
    [Test]
    public async Task RegisterNewUser()
    {
        Helper.TriggerRebuild();

        var newUser = new RegisterTestModel()
        {
            FullName = "Test User",
            Email = "user@example.com",
            Password = "password"
        };
            
        var url = "http://localhost:5000/api/account/register";
            
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync(url, newUser);
            TestContext.WriteLine("Body Response: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }

        ResponseTestModel responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<ResponseTestModel>(
                await response.Content.ReadAsStringAsync()) ?? throw new InvalidOperationException();
        }
        catch (Exception e)
        {
            throw new Exception(Helper.BadResponseBody(await response.Content.ReadAsStringAsync()), e);
        }

        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            responseObject.Should().BeEquivalentTo(new ResponseTestModel() {MessageToClient = "Registration successful."}, Helper.MyBecause(responseObject, new ResponseTestModel() {MessageToClient = "Registration successful."}));
        }
    }
    
    
    [TestCase("User with more than allowed name length", "test1@example.com", "password")]
    [TestCase("Valid Name", "test2-invalid-email.com", "password")]
    [TestCase("", "test3@example.com", "password")]
    [TestCase("Valid Name", "test4@example.com", "")]
    public async Task ShouldFailDueToDataValidation(string fullName, string email, string password)
    {
        var box = new RegisterTestModel()
        {
            FullName = fullName,
            Email = email,
            Password = password
        };
            
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/account/register", box);
            TestContext.WriteLine("Body Response: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }

        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}