using RiwiTalent.Application.DTOs;

namespace RiwiTalent.Domain.Services.Interface.Login
{
    public interface ILoginRepository
    {
        Task<ResponseJwtDto?> GenerateJwtCentinela(string tokenFirebase);
    }

}