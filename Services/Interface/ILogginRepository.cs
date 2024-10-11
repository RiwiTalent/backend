using RiwiTalent.Models;

namespace RiwiTalent.Services.Interface;

public interface ILogginRepository
{
    Task<ResponseJwt?> GenerateJwtCentinela(string tokenFirebase);
}