using System.Security.Claims;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Entities;
using RiwiTalent.Domain.Entities.Enums;
using RiwiTalent.Domain.ExternalKey;
using RiwiTalent.Domain.Services.Groups;
using RiwiTalent.Infrastructure.ExternalServices;
using RiwiTalent.Shared.Exceptions;

namespace RiwiTalent.Services.Repository
{
    #pragma warning disable
    public class GroupCoderRepository : IGroupCoderRepository
    {
        private readonly IMongoCollection<Group> _mongoCollection;
        private readonly IMongoCollection<Coder> _mongoCollectionCoder;
        private readonly ExternalKeyUtils _service;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string Error = "The group not found";
        public GroupCoderRepository(MongoDbContext context, IMapper mapper, ExternalKeyUtils service, IHttpContextAccessor httpContextAccessor)
        {
            _mongoCollection = context.Groups;
            _mongoCollectionCoder = context.Coders;
            _mapper = mapper;
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task Add(GroupDto groupDto)
        {
            var existGroup = await _mongoCollection.Find(g => g.Name == groupDto.Name).FirstOrDefaultAsync();

            if (existGroup != null)
            {
                throw new ApplicationException($"El grupo con el nombre '{groupDto.Name}' ya existe.");
            }

            Group groupCoder = new Group();

            //generate ObjectId
            ObjectId objectId = ObjectId.GenerateNewId();

            //we define the path of url link
            string tokenString = _service.GenerateTokenRandom();

            //define a new instance to add uuid into externalkeys -> url
            Group newGruopCoder = new Group
            {
                Id = objectId.ToString(),
                Name = groupDto.Name,
                Description = groupDto.Description,
                Created_At = DateTime.UtcNow,
                Deleted_At = null,
                Status = Status.Activo.ToString(),
                CreatedBy = groupDto.CreatedBy,
                AssociateEmail = groupDto.AssociateEmail,
                AcceptedTerms = false,
                ExternalKeys = new List<ExternalKey>
                {
                    new ExternalKey
                    {
                        Url =  $"https://riwi-talent.onrender.com/external-access/{objectId}",
                        Key = tokenString,
                        Status = Status.Activo.ToString(),
                        Date_Creation = DateTime.UtcNow,
                        Date_Expiration = DateTime.UtcNow.AddDays(15)
                    }
                },
            };

            await _mongoCollection.InsertOneAsync(newGruopCoder);
        }

        public async Task<KeyDto> SendToken(KeyDto keyDto)
        {
            try
            {
                Group gruopCoder = new Group();
                Group newGroupCoder = new Group
                {
                    Id = keyDto.Id,
                    AssociateEmail = keyDto.AssociateEmail,
                    ExternalKeys = new List<ExternalKey>
                    {
                        new ExternalKey
                        {
                            Key = keyDto.Key
                        }
                    }
                };

                var searchGroup = await _mongoCollection.Find(group => group.Id == keyDto.Id).FirstOrDefaultAsync();

                if(searchGroup == null)
                {
                    throw new StatusError.ObjectIdNotFound($"The document not found");
                }


                if(string.IsNullOrEmpty(searchGroup.AssociateEmail) || searchGroup.AssociateEmail.ToLower() != keyDto.AssociateEmail)
                {
                    throw new StatusError.EmailNotFound("The group hasn't valid Email or is empty");
                }
                else
                {
                    Console.WriteLine("The group has Email");
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

        public async Task RegenerateToken(string Id)
        {
            var group = await _mongoCollection.Find(g => g.Id == Id).FirstOrDefaultAsync();

            if(group == null)
                throw new StatusError.InvalidKeyException("Key isn't valid");
            
            string newKey = _service.GenerateTokenRandom();

            var filter = Builders<Group>.Filter.Eq(g => g.Id, group.Id);
            var updateKey = Builders<Group>.Update.Set(g => g.ExternalKeys[0].Key, newKey);

            await _mongoCollection.UpdateOneAsync(filter, updateKey);
        }


        public async Task<IEnumerable<GroupCoderDto>> GetGroupCoders()
        {
            var Groups = await _mongoCollection.Find(_ => true).ToListAsync();

            var newGroup = Groups.Select(groups => new GroupCoderDto
            {
                Id = groups.Id.ToString(),
                Name = groups.Name,
                Photo = groups.Photo,
                Description = groups.Description,
                Status = groups.Status,
                CreatedBy = groups.CreatedBy,
                Created_At = groups.Created_At = DateTime.UtcNow
            });

            return newGroup;
        }

        public async Task<GroupDetailsDto> GetGroupInfoById(string groupId)
        {
            var group = await _mongoCollection.Find(x => x.Id == groupId).FirstOrDefaultAsync();

            if(group == null)
            {
                throw new StatusError.ObjectIdNotFound("The group has not found");
            }

            var coders = await _mongoCollectionCoder.Find(x => x.GroupId.Contains(groupId.ToString()))
                .ToListAsync();
            
            List<CoderDto> coderMap = _mapper.Map<List<CoderDto>>(coders);
            CoderDto coderDto = new CoderDto();

            GroupDetailsDto groupInfo = new GroupDetailsDto()
            {
                Id = group.Id.ToString(),
                Name = group.Name,
                Photo = group.Photo,
                Description = group.Description,
                Status = group.Status,
                Created_At = group.Created_At,
                CreatedBy = group.CreatedBy,
                AssociateEmail = group.AssociateEmail,
                AcceptedTerms = group.AcceptedTerms,
                ExternalKeys= group.ExternalKeys,
                Coders = coderMap
            };

            return groupInfo;
        }
        
        // validation of group existence
        public async Task<bool> GroupExistByName(string name)
        {
            var filter = Builders<Group>.Filter.Regex(g => g.Name, name);
            var group = await _mongoCollection.Find(filter).FirstOrDefaultAsync();

            // Return true if group exists, if not return false
            return group != null;
        }

        public async Task ReactiveGroup(string id)
        {
            var filter = Builders<Group>.Filter.Eq(g => g.Id, id);
            
            var groupExists = await _mongoCollection.Find(filter).AnyAsync();
            if (!groupExists)
            {
                throw new KeyNotFoundException($"The group with ID {id} was not found.");
            }

            var update = Builders<Group>.Update.Set(g => g.Status, Status.Activo.ToString());
            await _mongoCollection.UpdateOneAsync(filter, update);
        }

        public async Task Update(GroupCoderDto groupCoderDto)
        {
            //we need filter groups by Id
            //First we call the method Builders and have access to Filter
            //Then we can use filter to have access Eq


            var existGroup = await _mongoCollection.Find(group => group.Id == groupCoderDto.Id).FirstOrDefaultAsync();

            if(existGroup == null)
            {
                throw new Exception($"{Error}");
            }

            var groupCoders = _mapper.Map(groupCoderDto, existGroup);
            var builder = Builders<Group>.Filter;
            var filter = builder.Eq(group => group.Id, groupCoderDto.Id );

            await _mongoCollection.ReplaceOneAsync(filter, groupCoders);
        }

        public async Task DeleteGroup(string groupId)
        {
            var filter = Builders<Group>.Filter.Eq(c => c.Id, groupId);         
            var updateStatusAndRelation = Builders<Group>.Update.Combine(
                Builders<Group>.Update.Set(coder => coder.Status, Status.Inactivo.ToString()),
                Builders<Group>.Update.Set(coder => coder.Deleted_At, DateTime.UtcNow)
            );

            var result = await _mongoCollection.UpdateOneAsync(filter, updateStatusAndRelation);
            if (result.ModifiedCount == 0)
            {
                throw new Exception("Group updated failed");
            }
        }

        public async Task<Group> GetGroupByName(string name)
        {
            var filter = Builders<Group>.Filter.Regex(g => g.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));

            return await _mongoCollection.Find(filter).FirstOrDefaultAsync();
        
        }

        public async Task<IEnumerable<Group>> GetGroupsInactive()
        {
            var filter = Builders<Group>.Filter.In(c => c.Status, new [] { Status.Inactivo.ToString()});
            return await _mongoCollection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Group>> GetGroupsActive()
        {
            var filter = Builders<Group>.Filter.In(c => c.Status, new [] { Status.Activo.ToString() });
            return await _mongoCollection.Find(filter).ToListAsync();
        }

        public Task<IEnumerable<Group>> GetGroupsInactivo()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Group>> GetGroupsActivo()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateGroupPhoto(string groupId, string photoUrl)
        {
            var filter = Builders<Group>.Filter.Eq(g => g.Id, groupId);
            var updatePhoto = Builders<Group>.Update.Set(g => g.Photo, photoUrl);
            await _mongoCollection.UpdateOneAsync(filter, updatePhoto);
        }
    }
}