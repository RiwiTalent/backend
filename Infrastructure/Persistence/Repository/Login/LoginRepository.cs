using System.Text;
using RiwiTalent.Domain.Services.Interface.Login;
using FirebaseAdmin.Auth;
using RiwiTalent.Application.DTOs;

namespace RiwiTalent.Infrastructure.Persistence.Repository;

public class LoginRepository : ILoginRepository
{
    private readonly HttpClient _httpClient;

    public LoginRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResponseJwtDto?> GenerateJwtCentinela(string tokenFirebase)
    {

        //we decode token of Firebase to get email
        FirebaseToken decodeToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(tokenFirebase);
        string email = decodeToken.Claims["email"].ToString();

        _httpClient.DefaultRequestHeaders.Add("x-api-key", "cd74b76960fc0448f55637ee5f5fd243ec8b0ae2148d320d022f93aec18ad685");
        
        string url = "https://dev.service.centinela.riwi.io/auth/login";

        try
        {
            var jsonContent = new StringContent($"{{\"firebaseToken\":\"{tokenFirebase}\"}}", Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await _httpClient.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                ResponseJwtDto responseBody = await response.Content.ReadFromJsonAsync<ResponseJwtDto>();

                responseBody.Email = email;

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