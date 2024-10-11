using System.Text;
using RiwiTalent.Models;
using RiwiTalent.Services.Interface;

namespace RiwiTalent.Services.Repository;

public class LogginRepository : ILogginRepository
{
    private readonly HttpClient _httpClient;

    public LogginRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResponseJwt?> GenerateJwtCentinela(string tokenFirebase)
    {
        _httpClient.DefaultRequestHeaders.Add("x-api-key", "cd74b76960fc0448f55637ee5f5fd243ec8b0ae2148d320d022f93aec18ad685");
        
        string url = "https://dev.service.centinela.riwi.io/auth/login";

        try
        {
            var jsonContent = new StringContent($"{{\"firebaseToken\":\"{tokenFirebase}\"}}", Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await _httpClient.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                ResponseJwt responseBody = await response.Content.ReadFromJsonAsync<ResponseJwt>();

                return responseBody;
            }
            else
            {
                return null;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}