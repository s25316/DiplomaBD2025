// Ignore Spelling: redis, dto

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.DTOs.Responses;
using UseCase.Shared.Interfaces;

namespace UseCase.Roles.Guests.Queries.Dictionaries.Repositories
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
        public async Task<Dictionary<int, CompanyRoleDto>> GetCompanyRolesAsync()
        {
            return await GetDataAsync<int, CompanyRoleDto, Role>(
                dto => dto.RoleId,
                () => _context.Roles,
                entity => entity.RoleId);
        }

        public async Task<Dictionary<int, CurrencyDto>> GetCurrenciesAsync()
        {
            return await GetDataAsync<int, CurrencyDto, Currency>(
                dto => dto.CurrencyId,
                () => _context.Currencies,
                entity => entity.CurrencyId);
        }

        public async Task<Dictionary<int, EmploymentTypeDto>> GetEmploymentTypesAsync()
        {
            return await GetDataAsync<int, EmploymentTypeDto, EmploymentType>(
                dto => dto.EmploymentTypeId,
                () => _context.EmploymentTypes,
                entity => entity.EmploymentTypeId);
        }

        public async Task<Dictionary<int, SalaryTermDto>> GetSalaryTermsAsync()
        {
            return await GetDataAsync<int, SalaryTermDto, SalaryTerm>(
                dto => dto.SalaryTermId,
                () => _context.SalaryTerms,
                entity => entity.SalaryTermId);
        }

        public async Task<Dictionary<int, WorkModeDto>> GetWorkModesAsync()
        {
            return await GetDataAsync<int, WorkModeDto, WorkMode>(
                dto => dto.WorkModeId,
                () => _context.WorkModes,
                entity => entity.WorkModeId);
        }

        public async Task<Dictionary<int, SkillTypeDto>> GetSkillTypesAsync()
        {
            return await GetDataAsync<int, SkillTypeDto, SkillType>(
                dto => dto.SkillTypeId,
                () => _context.SkillTypes,
                entity => entity.SkillTypeId);
        }

        public async Task<Dictionary<int, SkillResponseDto>> GetSkillsAsync()
        {
            var redisDictionary = await _redisService.GetAsync<int, SkillResponseDto>(dto => dto.SkillId);
            if (redisDictionary.Any())
            {
                return redisDictionary;
            }

            var dictionary = await _context.Skills
                .Include(skill => skill.SkillType)
                .ToDictionaryAsync(
                value => value.SkillId,
                value => _mapper.Map<SkillResponseDto>(value));

            await _redisService.SetAsync(dictionary);
            return dictionary;
        }

        private async Task<Dictionary<TKey, TDto>> GetDataAsync<TKey, TDto, TEntity>(
            Func<TDto, TKey> keySelectorTDto,
            Func<DbSet<TEntity>> Table,
            Func<TEntity, TKey> keySelectorTEntity)
            where TKey : notnull
            where TDto : class
            where TEntity : class
        {
            var redisDictionary = await _redisService.GetAsync<TKey, TDto>(keySelectorTDto);
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
