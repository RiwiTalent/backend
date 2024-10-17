using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Entities;
using RiwiTalent.Domain.Entities.Enums;
using RiwiTalent.Domain.Services.Interface.Coders;
using RiwiTalent.Infrastructure.ExternalServices;
using RiwiTalent.Shared.Exceptions;

namespace RiwiTalent.Infrastructure.Persistence.Repository
{
    public class CoderRepository : ICoderRepository
    {
        #pragma warning disable
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

        public async Task Add(CoderDto coderDto)
        {
            // Here we add a new coder
            var coder = _mapper.Map<Coder>(coderDto);
            await _mongoCollection.InsertOneAsync(coder);
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

        // Metodo para traer los coders
        public async Task<List<Coder>> GetCoders(List<string> skills)
        {
            // Filtrar todos los coders del repositorio
            if (skills == null || skills.Any())
            {
                // Crear un filtro para que coincidan todas las skills
                var filter = Builders<Coder>.Filter.AnyIn(c => c.Skills.Select(s => s.Language_Programming), skills);

                // Ejecutar la consulta con el filtro
                var coders = await _mongoCollection.Find(filter).ToListAsync();
                return coders;
            }

            // Si no se proporcionan skills, traer todos los coders
            var allCoders = await _mongoCollection.Find(Builders<Coder>.Filter.Empty).ToListAsync();
            return allCoders;
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

        public async Task Update(Coder coder)
        {

            var existCoder = await _mongoCollection.Find(c => c.Id == coder.Id).FirstOrDefaultAsync();

            if (existCoder == null)
            {
                throw new StatusError.ObjectIdNotFound($"{Error}");
            }

            var updateDefinition = new List<UpdateDefinition<Coder>>();

            if (!string.IsNullOrEmpty(coder.FirstName))
            {
                updateDefinition.Add(Builders<Coder>.Update.Set(c => c.FirstName, coder.FirstName));
            }
            if (!string.IsNullOrEmpty(coder.SecondName))
            {
                updateDefinition.Add(Builders<Coder>.Update.Set(c => c.SecondName, coder.SecondName));
            }
            if (!string.IsNullOrEmpty(coder.FirstLastName))
            {
                updateDefinition.Add(Builders<Coder>.Update.Set(c => c.FirstLastName, coder.FirstLastName));
            }
            if (!string.IsNullOrEmpty(coder.SecondLastName))
            {
                updateDefinition.Add(Builders<Coder>.Update.Set(c => c.SecondLastName, coder.SecondLastName));
            }
            if (!string.IsNullOrEmpty(coder.ProfessionalDescription))
            {
                updateDefinition.Add(Builders<Coder>.Update.Set(c => c.ProfessionalDescription, coder.ProfessionalDescription));
            }
            if (!string.IsNullOrEmpty(coder.Email))
            {
                updateDefinition.Add(Builders<Coder>.Update.Set(c => c.Email, coder.Email));
            }
            if (!string.IsNullOrEmpty(coder.Photo))
            {
                updateDefinition.Add(Builders<Coder>.Update.Set(c => c.Photo, coder.Photo));
            }
            if (!string.IsNullOrEmpty(coder.Phone))
            {
                updateDefinition.Add(Builders<Coder>.Update.Set(c => c.Phone, coder.Phone));
            }
            if (coder.Age > 0)
            {
                updateDefinition.Add(Builders<Coder>.Update.Set(c => c.Age, coder.Age));
            }
            if (coder.AssessmentScore > 0)
            {
                updateDefinition.Add(Builders<Coder>.Update.Set(c => c.AssessmentScore, coder.AssessmentScore));
            }
            if (!string.IsNullOrEmpty(coder.Cv))
            {
                updateDefinition.Add(Builders<Coder>.Update.Set(c => c.Cv, coder.Cv));
            }
            if (!string.IsNullOrEmpty(coder.Status))
            {
                updateDefinition.Add(Builders<Coder>.Update.Set(c => c.Status, coder.Status));
            }
            if (coder.GroupId != null && coder.GroupId.Any())
            {
                updateDefinition.Add(Builders<Coder>.Update.Set(c => c.GroupId, coder.GroupId));
            }
            if (!string.IsNullOrEmpty(coder.Stack))
            {
                updateDefinition.Add(Builders<Coder>.Update.Set(c => c.Stack, coder.Stack));
            }
            if (coder.StandarRiwi != null)
            {
                updateDefinition.Add(Builders<Coder>.Update.Set(c => c.StandarRiwi, coder.StandarRiwi));
            }
            if (coder.Skills != null && coder.Skills.Any())
            {
                updateDefinition.Add(Builders<Coder>.Update.Set(c => c.Skills, coder.Skills));
            }
            if (coder.LanguageSkills != null)
            {
                updateDefinition.Add(Builders<Coder>.Update.Set(c => c.LanguageSkills, coder.LanguageSkills));
            }

            // Actualiza la fecha de modificación
            updateDefinition.Add(Builders<Coder>.Update.Set(c => c.Date_Update, DateTime.UtcNow));

            // Combina todas las actualizaciones
            if (updateDefinition.Count > 0)
            {
                var update = Builders<Coder>.Update.Combine(updateDefinition);
                await _mongoCollection.UpdateOneAsync(c => c.Id == coder.Id, update);
            }
        }  

        public async Task UpdateCodersGroup(CoderGroupDto coderGroup)
        {
            await UpdateCodersProcess(coderGroup, Status.Grouped);
        }

        public async Task UpdateCodersSelected(CoderGroupDto coderGroup)
        {
            await UpdateCodersProcess(coderGroup, Status.Selected);
            var coders = await _mongoCollection.Find(x => x.GroupId.ToString() == coderGroup.GroupId && x.Status == Status.Grouped.ToString())
                .ToListAsync();
            

            await UpdateCodersProcess(coders, Status.Active);
        }

        public async Task Delete(string id)
        {
            //This Method is the reponsable of update status the coder, first we search by id and then it execute the change Active to Inactive

            var filter = Builders<Coder>.Filter.Eq(c => c.Id, id);         
            var update = Builders<Coder>.Update.Set(c => c.Status, Status.Inactive.ToString());            
            await _mongoCollection.UpdateOneAsync(filter, update);
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

        public async Task ReactivateCoder(string id)
        {
            //This Method is the reponsable of update status the coder, first we search by id and then it execute the change Inactive to Active
            
            var filter = Builders<Coder>.Filter.Eq(c => c.Id, id);           
            var update = Builders<Coder>.Update.Set(c => c.Status, Status.Active.ToString());
            await _mongoCollection.UpdateOneAsync(filter, update);
        }
        
        
        public async Task<List<Coder>> GetCodersBySkill(List<string> skill)
        {
            // This function aims to search for coder by skill
            try
            {
                var filter = new List<FilterDefinition<Coder>>();
                foreach (var language in skill)
                {
                    var languageFilter = Builders<Coder>.Filter.ElemMatch(c => c.Skills, s => s.Language_Programming == language);
                    filter.Add(languageFilter);
                }

                var combinedFilter = Builders<Coder>.Filter.And(filter);
                
                return await _mongoCollection.Find(combinedFilter).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("There is no coder with those languages.");
            }
        }

        public async Task<List<Coder>> GetCodersBylanguage([FromQuery]string level)
        {
            try
            {
                var filter = Builders<Coder>.Filter.Eq(c => c.LanguageSkills.Language_Level, level);
                return await _mongoCollection.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred getting coder", ex);
                
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
                existCoder = UpdateCoderInfo(existCoder, status, existCoder.GroupId.ToString());

                var filter = Builders<Coder>.Filter.Eq(x => x.Id, existCoder.Id);
                await _mongoCollection.ReplaceOneAsync(filter, existCoder);
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
                if (coder.GroupId == null)
                {
                    coder.GroupId = new List<string>();
                }

                if (!coder.GroupId.Contains(groupId))
                {
                    coder.GroupId.Add(groupId);
                }
                coder.Status = status.ToString();
            }

            return coder;
        }

        public async Task<Coder> FindCoderById(string coderId)
        {
            try
            {
                // we search coder by id, and if isn't exists return a exception
                var filter = Builders<Coder>.Filter.Eq(c => c.Id, coderId.ToString());
                var coder = await _mongoCollection.Find(filter).FirstOrDefaultAsync();

               
                if (coder == null)
                {
                    throw new StatusError.ObjectIdNotFound($"The coder with Id '{coderId}' not found.");
                }

                return coder;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred searching coder: {ex.Message}");
            }
        }

        public async Task UpdateCoderPhoto(string coderId, string photoUrl)
        {
            //This method have an important responsability of upload photo to each coder

            var filter = Builders<Coder>.Filter.Eq(c => c.Id, coderId);
            var updatePhoto = Builders<Coder>.Update.Set(c => c.Photo, photoUrl);
            await _mongoCollection.UpdateOneAsync(filter, updatePhoto);
        }

        public async Task UpdateCoderCv(string coderId, string pdf)
        {
            //This method have an important responsability of upload pdf cv to each coder

            var filter = Builders<Coder>.Filter.Eq(c => c.Id, coderId);
            var updatePdfCv= Builders<Coder>.Update.Set(c => c.Cv, pdf);
            await _mongoCollection.UpdateOneAsync(filter, updatePdfCv);
        }

        // public async Task<List<Coder>> FilterBySkills(List<string> selectedSkills)
        // {
        //     if (selectedSkills == null || !selectedSkills.Any())
        //         return new List<Coder>();

        //     // Filtro que busca coders que tengan alguna de las skills seleccionadas
        //     var filter = Builders<Coder>.Filter.AnyIn(c => c.Skills.Select(s => s.Language_Programming), selectedSkills);

        //     return await _mongoCollection.Find(filter).ToListAsync();
        // }
    }
}