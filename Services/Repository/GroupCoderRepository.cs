using System.Xml.Schema;
using AutoMapper;
using backend.Models.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Bson;
using MongoDB.Driver;
using RiwiTalent.Infrastructure.Data;
using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Models.Enums;
using RiwiTalent.Services.Interface;
using RiwiTalent.Utils.Exceptions;
using RiwiTalent.Utils.ExternalKey;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Amazon.Runtime.Internal.Settings;

namespace RiwiTalent.Services.Repository
{
    public class GroupCoderRepository : IGroupCoderRepository
    {
        private readonly IMongoCollection<GroupCoder> _mongoCollection  ;
        private readonly IMongoCollection<Coder> _mongoCollectionCoder;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ExternalKeyUtils _service;
        private readonly IMapper _mapper;

        private string Error = "The group not found";
        public GroupCoderRepository(MongoDbContext context, IMapper mapper, ExternalKeyUtils service, IHttpContextAccessor httpContextAccessor)
        {
            _mongoCollection = context.GroupCoders;
            _mongoCollectionCoder = context.Coders;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _service = service;
        }
        public void Add(GroupDto groupDto)
        {
            var existGroup = _mongoCollection.Find(g => g.Name == groupDto.Name).FirstOrDefault();
            var userEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

            if (existGroup != null)
            {
                throw new ApplicationException($"El grupo con el nombre '{groupDto.Name}' ya existe.");
            }

            GroupCoder groupCoder = new GroupCoder();

            //generate ObjectId
            ObjectId objectId = ObjectId.GenerateNewId();
            Guid guid =  _service.ObjectIdToUUID(objectId);
            groupCoder.Id = objectId;
              

            /* string RealObjectId = _service.RevertObjectIdUUID(guid);

            if(RealObjectId.ToString() == objectId.ToString())
                Console.WriteLine("Es igual");

            Console.WriteLine($"el objectId del grupo es: {RealObjectId}"); */


            //we define the path of url link
            string tokenString = _service.GenerateTokenRandom();

            //define a new instance to add uuid into externalkeys -> url
            GroupCoder newGruopCoder = new GroupCoder
            {
                Id = objectId,
                Name = groupDto.Name,
                Description = groupDto.Description,
                Created_At = DateTime.UtcNow,
                Deleted_At = null,
                Status = Status.Active.ToString(),
                CreatedBy = userEmail,
                AssociateEmail = groupDto.AssociateEmail,
                ExternalKeys = new List<ExternalKey>
                {
                    new ExternalKey
                    {
                        Url =  $"https://riwi-talent.onrender.com/{groupDto.Name}/{objectId}",
                        Key = tokenString,
                        Status = Status.Active.ToString(),
                        Date_Creation = DateTime.UtcNow,
                        Date_Expiration = DateTime.UtcNow.AddDays(7)
                    }
                },
            };

            _mongoCollection.InsertOne(newGruopCoder);
        }

        public async Task<KeyDto> SendToken(KeyDto keyDto)
        {
            try
            {
                GroupCoder gruopCoder = new GroupCoder();
                GroupCoder newGroupCoder = new GroupCoder
                {
                    Name = keyDto.Name,
                    ExternalKeys = new List<ExternalKey>
                    {
                        new ExternalKey
                        {
                            Key = keyDto.Key
                        }
                    }
                };

                var searchGroup = await _mongoCollection.Find(group => group.Name == keyDto.Name).FirstOrDefaultAsync();

                if(searchGroup == null)
                {
                    throw new StatusError.InvalidKeyException($"Name is invalid");
                }

                
                /* if(!string.IsNullOrEmpty(searchGroup.UUID))
                {
                    Console.WriteLine("The group has UUID");
                }
                else
                {
                    throw new Exception("The group hasn't valid UUID");
                } */

                if(searchGroup.ExternalKeys != null && searchGroup.ExternalKeys.Any())
                {
                    var KeyValidate = searchGroup.ExternalKeys.FirstOrDefault(k => k.Key.Trim().ToLower() == keyDto.Key.Trim().ToLower());

                    foreach (var item in searchGroup.ExternalKeys)
                    {
                        Console.WriteLine($"key disponible: {item.Key}");
                    }

                    if(KeyValidate != null)
                    {
                        return new KeyDto { Key = KeyValidate.Key };
                    }
                    else
                    {
                        
                        throw new StatusError.InvalidKeyException("The key isn't valid");
                    }
                }
                else 
                {
                    throw new Exception("External key not found in this group");
                }
            
                throw new StatusError.ExternalKeyNotFound("External key not found");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RegenerateToken(NewKeyDto newKeyDto)
        {
            var group = await _mongoCollection.Find(g => g.ExternalKeys.Any()).FirstOrDefaultAsync();

            if(group == null)
                throw new StatusError.InvalidKeyException("Key isn't valid");
            
            string newKey = _service.GenerateTokenRandom();

            var filter = Builders<GroupCoder>.Filter.Eq(g => g.Id, group.Id);
            var updateKey = Builders<GroupCoder>.Update.Set(g => g.ExternalKeys[0].Key, newKey);

            await _mongoCollection.UpdateOneAsync(filter, updateKey);
        }

        

        // public async Task DeleteCoderGroup(string id)
        // {
        //     var filterCoder = Builders<Coder>.Filter.Eq(coder => coder.Id, id);
        //     var updateStatusAndRelation = Builders<Coder>.Update.Combine(
        //         Builders<Coder>.Update.Set(coder => coder.Status, Status.Active.ToString()),
        //         Builders<Coder>.Update.Set(coder => coder.GroupId, null)
        //     );

        //     await _mongoCollectionCoder.UpdateOneAsync(filterCoder, updateStatusAndRelation);
        // }

        public async Task<IEnumerable<GroupCoderDto>> GetGroupCoders()
        {
            var Groups = await _mongoCollection.Find(_ => true).ToListAsync();

            var newGroup = Groups.Select(groups => new GroupCoderDto
            {
                Id = groups.Id.ToString(),
                Name = groups.Name,
                Description = groups.Description,
                Status = groups.Status,
                Created_At = groups.Created_At,
                Delete_At = groups.Deleted_At,
                ExternalKeys = groups.ExternalKeys
            });

            return newGroup;
        }

        public async Task<GroupInfoDto> GetGroupInfoById(string groupId)
        {
            var group = await _mongoCollection.Find(x => x.Id.ToString() == groupId).FirstOrDefaultAsync();

            if(group == null)
            {
                return null;
                // throw new Exception(Error);
            }

            var coders = await _mongoCollectionCoder.Find(x => x.GroupId == groupId)
                .ToListAsync();
            
            List<CoderDto> coderMap = _mapper.Map<List<CoderDto>>(coders);

            GroupInfoDto groupInfo = new GroupInfoDto()
            {
                Id = group.Id.ToString(),
                Name = group.Name,
                Description = group.Description,
                Coders = coderMap
            };

            return groupInfo;
        }
        
        // validation of group existence
        public async Task<bool> GroupExistByName(string name)
        {
            var filter = Builders<GroupCoder>.Filter.Regex(g => g.Name, name);
            var group = await _mongoCollection.Find(filter).FirstOrDefaultAsync();

            // Retorna true si el grupo existe, false si no
            return group != null;
        }

        public async Task Update(GroupCoderDto groupCoderDto)
        {
            //we need filter groups by Id
            //First we call the method Builders and have access to Filter
            //Then we can use filter to have access Eq

            var convertIdToObjectId = ObjectId.Parse(groupCoderDto.Id.ToString());

            var existGroup = await _mongoCollection.Find(group => group.Id == convertIdToObjectId).FirstOrDefaultAsync();

            if(existGroup == null)
            {
                throw new Exception($"{Error}");
            }

            var groupCoders = _mapper.Map(groupCoderDto, existGroup);
            var builder = Builders<GroupCoder>.Filter;
            var filter = builder.Eq(group => group.Id, convertIdToObjectId );

            await _mongoCollection.ReplaceOneAsync(filter, groupCoders);
        }

        public async Task DeleteGroup(string groupId)
        {
            var filter = Builders<GroupCoder>.Filter.Eq(c => c.Id, new ObjectId(groupId));         
            var updateStatusAndRelation = Builders<GroupCoder>.Update.Combine(
                Builders<GroupCoder>.Update.Set(coder => coder.Status, Status.Inactive.ToString()),
                Builders<GroupCoder>.Update.Set(coder => coder.Deleted_At, DateTime.UtcNow)
            );

            var result = await _mongoCollection.UpdateOneAsync(filter, updateStatusAndRelation);
            if (result.ModifiedCount == 0)
            {
                throw new Exception("No se pudo actualizar el grupo.");
            }
        }

        public async Task<IEnumerable<GroupCoder>> GetGroupsInactive()
        {
            var filter = Builders<GroupCoder>.Filter.In(c => c.Status, new [] { Status.Inactive.ToString()});
            return await _mongoCollection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<GroupCoder>> GetGroupsActive()
        {
            var filter = Builders<GroupCoder>.Filter.In(c => c.Status, new [] { Status.Active.ToString() });
            return await _mongoCollection.Find(filter).ToListAsync();
        }
    }
}