// Ignore Spelling: redis, dto

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.Dictionaries.GetContractParameters.Response;
using UseCase.Shared.Dictionaries.GetContractParameterTypes.Response;
using UseCase.Shared.Dictionaries.GetFaqs.Response;
using UseCase.Shared.Dictionaries.GetProcessTypes.Response;
using UseCase.Shared.Dictionaries.GetSkills.Response;
using UseCase.Shared.Dictionaries.GetSkillTypes.Response;
using UseCase.Shared.Dictionaries.GetUrlTypes.Response;
using UseCase.Shared.Interfaces;

namespace UseCase.Shared.Dictionaries.Repositories
{
    public class DictionariesRepository : IDictionariesRepository
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly IRedisService _redisService;
        private readonly IMapper _mapper;


        // Constructor
        public DictionariesRepository(
            DiplomaBdContext context,
            IRedisService redisService,
            IMapper mapper)
        {
            _context = context;
            _redisService = redisService;
            _mapper = mapper;
        }



        // Methods
        public async Task<IReadOnlyDictionary<int, ContractParameterDto>> GetContractParametersAsync()
        {
            return await GetDataAsync<int, ContractParameterDto, ContractParameter>(
                selector => selector.ContractParameterId,
                () => _context.ContractParameters
                .Include(pram => pram.ContractParameterType)
                .AsNoTracking(),
                entity => entity.ContractParameterId);
        }

        public async Task<IReadOnlyDictionary<int, ContractParameterTypeDto>> GetContractParameterTypesAsync()
        {
            return await GetDataAsync<int, ContractParameterTypeDto, ContractParameterType>(
                selector => selector.ContractParameterTypeId,
                () => _context.ContractParameterTypes.AsNoTracking(),
                entity => entity.ContractParameterTypeId);
        }

        public async Task<IReadOnlyDictionary<Guid, FaqDto>> GetFaqsAsync()
        {
            return await GetDataAsync<Guid, FaqDto, Faq>(
                selector => selector.FaqId,
                () => _context.Faqs.AsNoTracking(),
                entity => entity.FaqId);
        }

        public async Task<IReadOnlyDictionary<int, SkillDto>> GetSkillsAsync()
        {
            return await GetDataAsync<int, SkillDto, Skill>(
                selector => selector.SkillId,
                () => _context.Skills
                .Include(skill => skill.SkillType)
                .AsNoTracking(),
                entity => entity.SkillId);
        }

        public async Task<IReadOnlyDictionary<int, SkillTypeDto>> GetSkillTypesAsync()
        {
            return await GetDataAsync<int, SkillTypeDto, SkillType>(
                selector => selector.SkillTypeId,
                () => _context.SkillTypes.AsNoTracking(),
                entity => entity.SkillTypeId);
        }

        public async Task<IReadOnlyDictionary<int, UrlTypeDto>> GetUrlTypesAsync()
        {
            return await GetDataAsync<int, UrlTypeDto, UrlType>(
                selector => selector.UrlTypeId,
                () => _context.UrlTypes.AsNoTracking(),
                entity => entity.UrlTypeId);
        }

        public async Task<IReadOnlyDictionary<int, ProcessTypeDto>> GetProcessTypesAsync()
        {
            return await GetDataAsync<int, ProcessTypeDto, ProcessType>(
                selector => selector.ProcessTypeId,
                () => _context.ProcessTypes.AsNoTracking(),
                entity => entity.ProcessTypeId);
        }

        // Private Methods
        private async Task<Dictionary<TKey, TDto>> GetDataAsync<TKey, TDto, TEntity>(
            Func<TDto, TKey> keySelectorTDto,
            Func<IQueryable<TEntity>> Table,
            Func<TEntity, TKey> keySelectorTEntity)
            where TKey : notnull
            where TDto : class
            where TEntity : class
        {
            var redisDictionary = await _redisService.GetAsync(keySelectorTDto);
            if (redisDictionary.Any())
            {
                return redisDictionary;
            }

            var dictionary = await Table().ToDictionaryAsync(
                item => keySelectorTEntity(item),
                item => _mapper.Map<TDto>(item));

            await _redisService.SetAsync(dictionary);
            return dictionary;
        }
    }
}
