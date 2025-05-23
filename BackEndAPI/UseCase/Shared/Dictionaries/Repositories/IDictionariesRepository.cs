using UseCase.Shared.Dictionaries.GetContractParameters.Response;
using UseCase.Shared.Dictionaries.GetContractParameterTypes.Response;
using UseCase.Shared.Dictionaries.GetFaqs.Response;
using UseCase.Shared.Dictionaries.GetProcessTypes.Response;
using UseCase.Shared.Dictionaries.GetSkills.Response;
using UseCase.Shared.Dictionaries.GetSkillTypes.Response;
using UseCase.Shared.Dictionaries.GetUrlTypes.Response;

namespace UseCase.Shared.Dictionaries.Repositories
{
    public interface IDictionariesRepository
    {
        Task<IReadOnlyDictionary<int, ContractParameterDto>> GetContractParametersAsync();
        Task<IReadOnlyDictionary<int, ContractParameterTypeDto>> GetContractParameterTypesAsync();
        Task<IReadOnlyDictionary<Guid, FaqDto>> GetFaqsAsync();
        Task<IReadOnlyDictionary<int, SkillDto>> GetSkillsAsync();
        Task<IReadOnlyDictionary<int, SkillTypeDto>> GetSkillTypesAsync();
        Task<IReadOnlyDictionary<int, UrlTypeDto>> GetUrlTypesAsync();
        Task<IReadOnlyDictionary<int, ProcessTypeDto>> GetProcessTypesAsync();
    }
}
