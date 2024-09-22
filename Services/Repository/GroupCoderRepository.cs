using AutoMapper;
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

namespace RiwiTalent.Services.Repository
{
    public class GroupCoderRepository : IGroupCoderRepository
    {
        private readonly IMongoCollection<GruopCoder> _mongoCollection;
        private readonly IMongoCollection<Coder> _mongoCollectionCoder;
        private readonly ExternalKeyUtils _service;
        private readonly IMapper _mapper;

        private string Error = "The group not found";
        public GroupCoderRepository(MongoDbContext context, IMapper mapper, ExternalKeyUtils service)
        {
            _mongoCollection = context.GroupCoders;
            _mongoCollectionCoder = context.Coders;
            _mapper = mapper;
            _service = service;
        }
        public (ObjectId, Guid) Add(GroupDto groupDto)
        {
            var existGroup = _mongoCollection.Find(g => g.Name == groupDto.Name).FirstOrDefault();

            if (existGroup != null)
            {
                throw new ApplicationException($"El grupo con el nombre '{groupDto.Name}' ya existe.");
            }

            GruopCoder groupCoder = new GruopCoder(); 

            //generate ObjectId
            ObjectId objectId = ObjectId.GenerateNewId();
            Guid guid =  _service.ObjectIdToUUID(objectId);
            groupCoder.Id = objectId;
              

            string RealObjectId = _service.RevertObjectIdUUID(guid);

            Console.WriteLine($"el objectId del grupo es: {RealObjectId}");


            //we define the path of url link
            string Link = $"http://riwitalent/external/{guid}";
            string tokenString = _service.GenerateTokenRandom();

            //define a new instance to add uuid into externalkeys -> url
            GruopCoder newGruopCoder = new GruopCoder
            {
                Id = objectId,
                Name = groupDto.Name,
                Description = groupDto.Description,
                Created_At = DateTime.UtcNow,
                Status = Status.Active.ToString(),
                UUID = guid.ToString(),
                ExternalKeys = new List<ExternalKey>
                {
                    new ExternalKey
                    {
                        Url = Link,
                        Key = tokenString,
                        Status = Status.Active.ToString(),
                        Date_Creation = DateTime.UtcNow,
                        Date_Expiration = DateTime.UtcNow.AddDays(7)
                    }
                },
            };

            _mongoCollection.InsertOne(newGruopCoder);

            return (groupCoder.Id, guid);
        }

        public async Task<KeyDto> SendToken(GruopCoder gruopCoder, string key)
        {
            try
            {
                var searchGroup = await _mongoCollection.Find(group => group.UUID == gruopCoder.UUID).FirstOrDefaultAsync();

                if(searchGroup == null)
                {
                    throw new Exception($"Id is invalid");
                }

                if(searchGroup.ExternalKeys != null && searchGroup.ExternalKeys.Any())
                {
                    var KeyValidate = searchGroup.ExternalKeys.FirstOrDefault(k => k.Key.Trim().ToLower() == key.Trim().ToLower());

                    foreach (var item in searchGroup.ExternalKeys)
                    {
                        Console.WriteLine($"key disponible: {item.Key}");
                    }

                    if(KeyValidate != null)
                    {
                        Console.WriteLine("The key is correct");
                        return new KeyDto { Key = KeyValidate.Key };
                    }
                    else
                    {
                        StatusError.CreateNotFound();
                    }
                }
                else 
                {
                    throw new Exception("External key not found in this group");
                }
            
                throw new Exception("External key not found");
            }
            catch (Exception)
            {
                throw;
            }
        }

        

        public async Task DeleteCoderGroup(string id)
        {
            var filterCoder = Builders<Coder>.Filter.Eq(coder => coder.Id, id);
            var updateStatusAndRelation = Builders<Coder>.Update.Combine(
                Builders<Coder>.Update.Set(coder => coder.Status, Status.Inactive.ToString()),
                Builders<Coder>.Update.Set(coder => coder.GroupId, null)
            );

            await _mongoCollectionCoder.UpdateOneAsync(filterCoder, updateStatusAndRelation);
        }

        public async Task<IEnumerable<GroupCoderDto>> GetGroupCoders()
        {
            var Groups = await _mongoCollection.Find(_ => true).ToListAsync();

            var newGroup = Groups.Select(groups => new GroupCoderDto
            {
                Id = groups.Id.ToString(),
                Name = groups.Name,
                Description = groups.Description,
                Created_At = groups.Created_At,
                ExternalKeys = groups.ExternalKeys
            });

            return newGroup;
        }

        public async Task<GroupInfoDto> GetGroupInfoById(string groupId)
        {
            var group = await _mongoCollection.Find(x => x.Id.ToString() == groupId).FirstOrDefaultAsync();

            if(group == null)
            {
                throw new Exception(Error);
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
        public async Task<IEnumerable<GruopCoder>> GroupExistByName(string name)
        {
            var groups = await _mongoCollection.Find(group => group.Name == name).ToListAsync();

            if(groups.Count == 0)
                throw new Exception("The group name not exists");

            var newGroup = groups.Select(groups => new GruopCoder
            {
                Id = groups.Id,
                Name = groups.Name,
                Description = groups.Description,
                Status = groups.Status,
                Created_At = groups.Created_At,
                UUID = groups.UUID,
                Coders = groups.Coders,
                ExternalKeys = groups.ExternalKeys
            });

            
            return newGroup;

            /* var filter = Builders<GruopCoder>.Filter.Eq(g => g.Name, name);
            var group = await _mongoCollection.Find(filter).FirstOrDefaultAsync();

            // Retorna true si el grupo existe, false si no
            return group != null; */
        }

        public async Task Update(GroupCoderDto groupCoderDto)
        {
            //we need filter groups by Id
            //First we call the method Builders and have access to Filter
            //Then we can use filter to have access Eq

            var convertIdToObjectId = ObjectId.Parse(groupCoderDto.Id);

            var existGroup = await _mongoCollection.Find(group => group.Id == convertIdToObjectId).FirstOrDefaultAsync();

            if(existGroup == null)
            {
                throw new Exception($"{Error}");
            }

            var groupCoders = _mapper.Map(groupCoderDto, existGroup);
            var builder = Builders<GruopCoder>.Filter;
            var filter = builder.Eq(group => group.Id, convertIdToObjectId );

            await _mongoCollection.ReplaceOneAsync(filter, groupCoders);
        }
    }
}