﻿using UseCase.Shared.DTOs.Responses.Dictionaries;

namespace UseCase.Roles.Guests.Queries.Dictionaries.Repositories
{
    public interface IDictionariesRepository
    {
        Task<Dictionary<int, CompanyRoleDto>> GetCompanyRolesAsync();
        Task<Dictionary<int, CurrencyDto>> GetCurrenciesAsync();
        Task<Dictionary<int, EmploymentTypeDto>> GetEmploymentTypesAsync();
        Task<Dictionary<int, SalaryTermDto>> GetSalaryTermsAsync();
        Task<Dictionary<int, WorkModeDto>> GetWorkModesAsync();
        Task<Dictionary<int, SkillTypeDto>> GetSkillTypesAsync();
        Task<Dictionary<int, SkillResponseDto>> GetSkillsAsync();
        Task<Dictionary<int, NotificationTypeDto>> GetNotificationTypesAsync();
        Task<Dictionary<int, UrlTypeDto>> GetUrlTypesAsync();
        Task<Dictionary<Guid, FaqDto>> GetFaqsAsync();
    }
}
