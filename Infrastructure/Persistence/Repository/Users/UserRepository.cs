using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Entities;
using RiwiTalent.Domain.Services.Interface.Users;
using RiwiTalent.Infrastructure.ExternalServices;
using RiwiTalent.Shared.Exceptions;

namespace RiwiTalent.Infrastructure.Persistence.Repository.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _mongoCollection;
        private readonly IMapper _mapper;
        public UserRepository(MongoDbContext context, IMapper mapper)
        {
            _mongoCollection = context.Users;
            _mapper = mapper;
        }
        public async Task Add(User user)
        {
            await _mongoCollection.InsertOneAsync(user);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _mongoCollection.Find(_ => true).ToListAsync();

            return users;
        }

        public async Task Update(UserDto userDto)
        {
            var searchUser = await _mongoCollection.Find(u => u.Id == userDto.Id).FirstOrDefaultAsync();

            if(searchUser == null)
                throw new StatusError.ObjectIdNotFound("user not found");

            var userMap = _mapper.Map(userDto, searchUser);
            var filter = Builders<User>.Filter.Eq(u => u.Id, userMap.Id);

            await _mongoCollection.ReplaceOneAsync(filter, userMap);

        }
    }
}