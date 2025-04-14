using Domain.Features.Companies.ValueObjects.Ids;
using Domain.Features.ContractConditions.Entities;
using Domain.Features.ContractConditions.Exceptions;
using Domain.Features.ContractConditions.ValueObjects.HoursPerTerms;
using Domain.Features.ContractConditions.ValueObjects.Ids;
using Domain.Features.ContractConditions.ValueObjects.Info;
using Domain.Features.ContractConditions.ValueObjects.SalaryRanges;
using Domain.Shared.CustomProviders;
using Domain.Shared.Templates;

namespace Domain.Features.ContractConditions.Aggregates
{
    public partial class ContractCondition : TemplateEntity<ContractConditionId>
    {
        // Properties
        public CompanyId CompanyId { get; private set; } = null!;
        public SalaryRange SalaryRange { get; private set; } = null!;
        public HoursPerTerm HoursPerTerm { get; private set; } = null!;
        public bool IsNegotiable { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Removed { get; private set; } = null;
        // Collections
        private Dictionary<int, ContractAttribute> _salaryTerms = [];
        public IReadOnlyDictionary<int, ContractAttribute> SalaryTerms => _salaryTerms;
        private Dictionary<int, ContractAttribute> _currencies = [];
        public IReadOnlyDictionary<int, ContractAttribute> Currencies => _currencies;
        private Dictionary<int, ContractAttribute> _workModes = [];
        public IReadOnlyDictionary<int, ContractAttribute> WorkModes => _workModes;
        private Dictionary<int, ContractAttribute> _employmentTypes = [];
        public IReadOnlyDictionary<int, ContractAttribute> EmploymentTypes => _employmentTypes;


        // Public Methods
        public void Remove()
        {
            Removed = Removed.HasValue
                ? null
                : CustomTimeProvider.Now;
        }

        // Private Methods
        private void SetSalaryRange(
                decimal minSalary,
                decimal maxSalary)
        {
            SalaryRange = new SalaryRange(minSalary, maxSalary);
        }

        private void SetContractParameters(
            ContractAttributeInfo? salaryTerm,
            ContractAttributeInfo? currency,
            IEnumerable<ContractAttributeInfo> workModeIds,
            IEnumerable<ContractAttributeInfo> employmentTypeIds)
        {
            SetContractAttribute(
                salaryTerm,
                _salaryTerms,
                Messages.Entity_ContractCondition_SalaryTermId_Empty);
            SetContractAttribute(
                currency,
                _currencies,
                Messages.Entity_ContractCondition_CurrencyId_Empty);

            SetContractAttributes(workModeIds, _workModes);
            SetContractAttributes(employmentTypeIds, _employmentTypes);
        }

        private void SetContractAttribute(
            ContractAttributeInfo? attribute,
            Dictionary<int, ContractAttribute> dictionary,
            string exceptionMessage)
        {
            // If Not payed set all on Removed
            if (SalaryRange == null || SalaryRange.Max <= 0)
            {
                foreach (var item in dictionary.Values)
                {
                    item.Remove();
                }
                return;
            }

            //if Payed
            if (attribute != null)
            {
                // If not exist Set
                if (!dictionary.ContainsKey(attribute.ContractParameterId))
                {
                    dictionary[attribute.ContractParameterId] = Map(attribute);
                }

                foreach (var item in dictionary.Values)
                {
                    // For all Set Removed
                    if (item.ContractParameterId != attribute.ContractParameterId)
                    {
                        item.Remove();
                    }
                    else
                    {
                        // For New Removed Null
                        item.Removed = null;
                    }
                }
            }
            else
            {
                // Check is Exist if Payed, if not Exception
                _ = dictionary.Values
                    .FirstOrDefault(i => i.Removed == null)
                    ?? throw new ContractConditionException(exceptionMessage);
            }
        }

        private void SetContractAttributes(
            IEnumerable<ContractAttributeInfo> attributes,
            Dictionary<int, ContractAttribute> dictionary)
        {
            var attributesDictionary = attributes
                .ToDictionary(a => a.ContractParameterId);

            var attributesKeys = attributesDictionary.Keys.ToHashSet();
            var dictionaryKeys = dictionary.Keys.ToHashSet();

            var intersectKeys = dictionaryKeys.Intersect(attributesKeys);
            var removedKeys = dictionaryKeys.Except(intersectKeys);
            var newKeys = attributesKeys.Except(intersectKeys);

            foreach (var key in removedKeys)
            {
                dictionary[key].Remove();
            }

            foreach (var key in newKeys)
            {
                dictionary[key] = Map(attributesDictionary[key]);
            }
        }

        private ContractAttribute Map(ContractAttributeInfo item)
        {
            return new ContractAttribute
            {
                Id = item.Id,
                ContractParameterId = item.ContractParameterId,
                Created = item.Created ?? CustomTimeProvider.Now,
                Removed = item.Removed,
            };
        }

    }
}
