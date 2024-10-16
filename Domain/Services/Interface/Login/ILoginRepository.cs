using RiwiTalent.Domain.Entities;

namespace RiwiTalent.Domain.Services.Interface.Login
{
    public interface ILoginRepository
    {
        Task<ResponseJwt?> GenerateJwtCentinela(string tokenFirebase);
    }

}