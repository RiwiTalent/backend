using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RiwiTalent.Infrastructure.Data;
using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Models.Enums;
using RiwiTalent.Services.Interface;

namespace RiwiTalent.Services.Repository
{
    public class CoderRepository : ICoderRepository
    {
        private readonly IMongoCollection<Coder> _mongoCollection;
        private readonly IMongoCollection<Group> _mongoCollectionGroups;
        private readonly IMapper _mapper; 
        private string Error = "The coder not found";
        public CoderRepository(MongoDbContext context, IMapper mapper)
        {
            _mongoCollection = context.Coders;
            _mongoCollectionGroups = context.Groups;
            _mapper = mapper;
        }

        public void Add(CoderDto coderDto)
        {
            // Mapeo de CoderDto a Coder
            var coder = _mapper.Map<Coder>(coderDto);
            _mongoCollection.InsertOne(coder);
        }

        public async Task<Coder> GetCoderId(string id)
        {
            //In this method we get coders by id and we do a control of errors.
            return await _mongoCollection.Find(Coders => Coders.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Coder> GetCoderName(string name)
        {
            //In this method we get coders by name and we do a control of errors.
            try
            {
                return await _mongoCollection.Find(Coders => Coders.FirstName == name).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("Ocurrió un error al obtener el coder", ex);
            }
        }

        public async Task<IEnumerable<Coder>> GetCoders()
        {
            var coders = await _mongoCollection.Find(_ => true)
                                                .ToListAsync();
            
            return coders;
        }

        public async Task<Pagination<Coder>> GetCodersPagination(int page, int cantRegisters)
        {
            var skip = (page -1) * cantRegisters;
            //we get all coders
            var coders = await _mongoCollection.Find(_ => true)
                                                .Skip(skip)
                                                .Limit(cantRegisters)
                                                .ToListAsync();

            var total = await _mongoCollection.CountDocumentsAsync(_ => true);                                  
            return Pagination<Coder>.CreatePagination(coders, (int)total, page, cantRegisters);
        }

        public async Task Update(CoderDto coderDto)
        {
            //we need filter groups by Id
            //First we call the method Builders and have access to Filter
            //Then we can use filter to have access Eq

            var existCoder = await _mongoCollection.Find(coder => coder.Id == coderDto.Id).FirstOrDefaultAsync();

            if(existCoder is null)
            {
                throw new Exception($"{Error}");
            }

            var coderMap = _mapper.Map(coderDto, existCoder);
            var builder = Builders<Coder>.Filter;
            var filter = builder.Eq(coder => coder.Id, coderMap.Id);

            await _mongoCollection.ReplaceOneAsync(filter, coderMap);
        }  

        public async Task UpdateCodersGroup(CoderGroupDto coderGroup)
        {
            await UpdateCodersProcess(coderGroup, Status.Grouped);
        }

        public async Task UpdateCodersSelected(CoderGroupDto coderGroup)
        {
            await UpdateCodersProcess(coderGroup, Status.Selected);
            var coders = await _mongoCollection.Find(x => x.GroupId == coderGroup.GroupId && x.Status == Status.Grouped.ToString())
                .ToListAsync();

            await UpdateCodersProcess(coders, Status.Active);
        }

        public void Delete(string id)
        {
            //This Method is the reponsable of update status the coder, first we search by id and then it execute the change Active to Inactive

            var filter = Builders<Coder>.Filter.Eq(c => c.Id, id);         
            var update = Builders<Coder>.Update.Set(c => c.Status, Status.Inactive.ToString());            
            _mongoCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteCoderGroup(string id)
        {
            var filterCoder = Builders<Coder>.Filter.Eq(coder => coder.Id, id);
            var updateStatusAndRelation = Builders<Coder>.Update.Combine(
                Builders<Coder>.Update.Set(coder => coder.Status, Status.Active.ToString()),
                Builders<Coder>.Update.Set(coder => coder.GroupId, null)
            );

            await _mongoCollection.UpdateOneAsync(filterCoder, updateStatusAndRelation);
        }

        public void ReactivateCoder(string id)
        {
            //This Method is the reponsable of update status the coder, first we search by id and then it execute the change Inactive to Active
            
            var filter = Builders<Coder>.Filter.Eq(c => c.Id, id);           
            var update = Builders<Coder>.Update.Set(c => c.Status, Status.Active.ToString());
            _mongoCollection.UpdateOne(filter, update);
        }
        
        
        public async Task<List<Coder>> GetCodersBySkill(List<string> skill)
        {
            try
            {
                var filter = new List<FilterDefinition<Coder>>(); //Defino una variable en la cual ingreso a un listado de lenguajes
                foreach (var language in skill) //Hago un foreach para recorrer todos los lenguajes de programacion y de esta forma verificar que el coder lo tiene
                {
                    var languageFilter = Builders<Coder>.Filter.ElemMatch(c => c.Skills, s => s.Language_Programming == language);
                    filter.Add(languageFilter); //Cada que voy obteniendo coders con las skills los añado a languageFilter 
                }

                var combinedFilter = Builders<Coder>.Filter.And(filter); //Luego junto las variables filter y langugeFilter para luego returnarlo
                
                return await _mongoCollection.Find(combinedFilter).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("No hay coder con esos lenguajes.");
            }
        }

        public async Task<List<Coder>> GetCodersBylanguage([FromQuery]string level)
        {
            try
            {
                var filter = Builders<Coder>.Filter.Eq(c => c.LanguageSkills.Language_Level, level); //Busco language_Level dentro de LanguageSkills y creo el filtrado
                return await _mongoCollection.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("Ocurrió un error al obtener el coder", ex);
                
             }
         }

        private async Task UpdateCodersProcess(CoderGroupDto coderGroup, Status status)
        {
            List<string> coderIdList = coderGroup.CoderList;
            for (int i = 0; i < coderIdList.Count; i++)
            {
                string coderId = coderIdList[i];
                var existCoder = await _mongoCollection.Find(coder => coder.Id == coderId).FirstOrDefaultAsync();
                
                if(existCoder is null)
                    throw new Exception($"{Error}");

                existCoder = UpdateCoderInfo(existCoder, status, coderGroup.GroupId);

                var filter = Builders<Coder>.Filter.Eq(x => x.Id, coderId);

                await _mongoCollection.ReplaceOneAsync(filter, existCoder);
            }
        }

        private async Task UpdateCodersProcess(List<Coder> coders, Status status)
        {
            for (int i = 0; i < coders.Count; i++)
            {
                Coder existCoder = coders[i];
                
                if(existCoder is null)
                    throw new Exception($"{Error}");

                // TASK: Se deberia de hacer un automapper
                // existCoder.GroupId = "";
                existCoder = UpdateCoderInfo(existCoder, status, existCoder.GroupId);

                var filter = Builders<Coder>.Filter.Eq(x => x.Id, existCoder.Id);
                await _mongoCollection.ReplaceOneAsync(filter, existCoder);
            }
        }

        public async Task<IEnumerable<Coder>> GetCodersByGroup(string name)
        {
            try
            {
                // first we get the name
                var group = await _mongoCollectionGroups.Find(g => g.Name == name).FirstOrDefaultAsync();

                if (group == null)
                {
                    throw new ApplicationException($"El grupo con el nombre '{name}' no existe.");
                }

                // then we compare the coder group id with the group id
                var filter = Builders<Coder>.Filter.Eq(c => c.GroupId, group.Id.ToString());

                var codersList = await _mongoCollection.Find(filter).ToListAsync();

                return codersList;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener los coders del grupo '{name}'", ex);
            }
        }

        private Coder UpdateCoderInfo(Coder coder, Status status, string groupId)
        {
            if(status.Equals(Status.Active))
            {
                coder.GroupId = null;
                coder.Status = Status.Active.ToString();
            }

            if(status.Equals(Status.Selected) || status.Equals(Status.Grouped))
            {
                coder.GroupId = groupId;
                coder.Status = status.ToString();
            }

            return coder;
        }
    }
}