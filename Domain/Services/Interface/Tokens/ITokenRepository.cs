using RiwiTalent.Domain.Entities;

namespace RiwiTalent.Domain.Services.Tokens
{
    public interface ITokenRepository
    {
        Task<string> GetToken(User user);
    }
}