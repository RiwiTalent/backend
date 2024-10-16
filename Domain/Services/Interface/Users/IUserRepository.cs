using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Entities;

namespace RiwiTalent.Domain.Services.Interface.Users
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task Add(User user);
        Task Update(UserDto userDto);
    }
}