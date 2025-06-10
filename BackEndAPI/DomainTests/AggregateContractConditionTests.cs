using Domain.Features.ContractConditions.Aggregates;
using Domain.Features.ContractConditions.ValueObjects.Info;
using Domain.Shared.Enums;
using Domain.Shared.Exceptions;

namespace DomainTests
{
    public class AggregateContractConditionTests
    {
        // Methods
        public static IEnumerable<object[]> TestData_ContractConditionBuilder_Correct_Correct
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    12,
                    13,
                    1,
                    true,
                    (DateTime?) null,
                    (DateTime?) null,
                    1,
                    1,
                    (IEnumerable<int> )[1,2,3,4],
                    (IEnumerable<int> )[1,2,3,4],
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    0,
                    0,
                    1,
                    true,
                    (DateTime?) DateTime.Now,
                    (DateTime?) DateTime.Now,
                    1,
                    1,
                    (IEnumerable<int> )[1,2,3,4],
                    (IEnumerable<int> )[1,2,3,4],
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    (decimal?)null,
                    (decimal?) null,
                    (int?) null,
                    true,
                    (DateTime?) DateTime.Now,
                    (DateTime?) DateTime.Now,
                    1,
                    1,
                    (IEnumerable<int> )[1,2,3,4],
                    (IEnumerable<int> )[1,2,3,4],
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_ContractConditionBuilder_Correct_Correct))]
        public void ContractConditionBuilder_Correct_Correct(
            Guid id,
            Guid companyId,
            decimal salaryMin,
            decimal salaryMax,
            int? hoursPerTerm,
            bool isNegotiable,
            DateTime? created,
            DateTime? removed,
            int? salaryTerm,
            int? currency,
            IEnumerable<int> workMode,
            IEnumerable<int> employmentType)
        {
            var builder = new ContractCondition.Builder()
                .SetId(id)
                .SetCompanyId(companyId)
                .SetSalaryRange(salaryMin, salaryMax)
                .SetIsNegotiable(isNegotiable)
                .SetRemoved(removed)
                .SetContractParameters(
                    (ContractAttributeInfo?)salaryTerm,
                    (ContractAttributeInfo?)currency,
                    workMode.Select(x => (ContractAttributeInfo?)x),
                    employmentType.Select(x => (ContractAttributeInfo?)x));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }
            if (hoursPerTerm.HasValue)
            {
                builder.SetHoursPerTerm(hoursPerTerm.Value);
            }

            Assert.False(builder.HasErrors());
            var item = builder.Build();
            Assert.NotNull(item.Created);

            Assert.Equal(workMode.Count(), item.WorkModes.Count);
            Assert.Equal(employmentType.Count(), item.EmploymentTypes.Count);

            if (salaryMax == null || salaryMax == 0)
            {
                Assert.Equal(0, item.SalaryTerms.Count);
                Assert.Equal(0, item.Currencies.Count);
            }
            else
            {
                Assert.Equal(1, item.SalaryTerms.Count);
                Assert.Equal(1, item.Currencies.Count);
            }
        }


        public static IEnumerable<object[]> TestData_ContractConditionBuilder_CorrectNullSalaryRange_Correct
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    (decimal?)null,
                    (decimal?) null,
                    (int?) null,
                    true,
                    (DateTime?) DateTime.Now,
                    (DateTime?) DateTime.Now,
                    1,
                    1,
                    (IEnumerable<int> )[1,2,3,4],
                    (IEnumerable<int> )[1,2,3,4],
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_ContractConditionBuilder_CorrectNullSalaryRange_Correct))]
        public void ContractConditionBuilder_CorrectNullSalaryRange_Correct(
            Guid id,
            Guid companyId,
            decimal? salaryMin,
            decimal? salaryMax,
            int? hoursPerTerm,
            bool isNegotiable,
            DateTime? created,
            DateTime? removed,
            int? salaryTerm,
            int? currency,
            IEnumerable<int> workMode,
            IEnumerable<int> employmentType)
        {
            var builder = new ContractCondition.Builder()
                .SetId(id)
                .SetCompanyId(companyId)
                .SetIsNegotiable(isNegotiable)
                .SetRemoved(removed)
                .SetContractParameters(
                    (ContractAttributeInfo?)salaryTerm,
                    (ContractAttributeInfo?)currency,
                    workMode.Select(x => (ContractAttributeInfo?)x),
                    employmentType.Select(x => (ContractAttributeInfo?)x));


            Assert.False(builder.HasErrors());
            var item = builder.Build();
            Assert.Equal(0, item.SalaryRange.Min);
            Assert.Equal(0, item.SalaryRange.Max);
        }


        [Fact]
        public void ContractConditionBuilder_NullCompanyId_TemplateBuilderException()
        {
            var result = Assert.Throws<TemplateBuilderException>(
                () => new ContractCondition.Builder().Build());
            Assert.Equal(HttpCode.InternalServerError, result.Code);
        }



        public static IEnumerable<object[]> TestData_ContractCondition_Correct
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    12,
                    13,
                    1,
                    true,
                    DateTime.Now,
                    (DateTime?) null,
                    1,
                    1,
                    (IEnumerable<int> )[1,2,3,4],
                    (IEnumerable<int> )[1,2,3,4],
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_ContractCondition_Correct))]
        public void ContractConditionUpdater_SalaryRangeIsZero_Correct(
            Guid id,
            Guid companyId,
            decimal salaryMin,
            decimal salaryMax,
            int hoursPerTerm,
            bool isNegotiable,
            DateTime created,
            DateTime? removed,
            int? salaryTerm,
            int? currency,
            IEnumerable<int> workMode,
            IEnumerable<int> employmentType)
        {
            var builder = new ContractCondition.Builder()
                .SetId(id)
                .SetCompanyId(companyId)
                .SetSalaryRange(salaryMin, salaryMax)
                .SetHoursPerTerm(hoursPerTerm)
                .SetIsNegotiable(isNegotiable)
                .SetCreated(created)
                .SetRemoved(removed)
                .SetContractParameters(
                    (ContractAttributeInfo?)salaryTerm,
                    (ContractAttributeInfo?)currency,
                    workMode.Select(x => (ContractAttributeInfo?)x),
                    employmentType.Select(x => (ContractAttributeInfo?)x));

            Assert.False(builder.HasErrors());
            var item = builder.Build();

            var updater = new ContractCondition.Updater(item)
                .SetSalaryRange(0, 0)
                .SetHoursPerTerm(12)
                .SetIsNegotiable(false)
                .SetContractParameters(
                 null,
                 null,
                 [],
                 []);
            var item2 = updater.Build();

            Assert.Equal(
                (salaryTerm == null ? 0 : 1),
                item2.SalaryTerms.Values.Where(x => x.Removed.HasValue).Count()
                );

            Assert.Equal(
                (currency == null ? 0 : 1),
                item2.Currencies.Values.Where(x => x.Removed.HasValue).Count()
                );
            Assert.Equal(
                workMode.Count(),
                item2.WorkModes.Values.Where(x => x.Removed.HasValue).Count()
                );
            Assert.Equal(
                employmentType.Count(),
                item2.EmploymentTypes.Values.Where(x => x.Removed.HasValue).Count()
                );
        }


        [Theory]
        [MemberData(nameof(TestData_ContractCondition_Correct))]
        public void ContractConditionUpdater_Correct_Correct(
            Guid id,
            Guid companyId,
            decimal salaryMin,
            decimal salaryMax,
            int hoursPerTerm,
            bool isNegotiable,
            DateTime created,
            DateTime? removed,
            int? salaryTerm,
            int? currency,
            IEnumerable<int> workMode,
            IEnumerable<int> employmentType)
        {
            var builder = new ContractCondition.Builder()
                .SetId(id)
                .SetCompanyId(companyId)
                .SetSalaryRange(salaryMin, salaryMax)
                .SetHoursPerTerm(hoursPerTerm)
                .SetIsNegotiable(isNegotiable)
                .SetCreated(created)
                .SetRemoved(removed)
                .SetContractParameters(
                    (ContractAttributeInfo?)salaryTerm,
                    (ContractAttributeInfo?)currency,
                    workMode.Select(x => (ContractAttributeInfo?)x),
                    employmentType.Select(x => (ContractAttributeInfo?)x));

            Assert.False(builder.HasErrors());
            var item = builder.Build();

            var updater = new ContractCondition.Updater(item)
                .SetContractParameters(
                 2,
                 2,
                 [],
                 []);
            var item2 = updater.Build();


            Assert.Equal(
                (salaryTerm == null ? 0 : 1),
                item2.SalaryTerms.Values.Where(x => x.Removed.HasValue).Count()
                );

            Assert.Equal(
                (currency == null ? 0 : 1),
                item2.Currencies.Values.Where(x => x.Removed.HasValue).Count()
                );
            Assert.Equal(2, item2.SalaryTerms.Count);
            Assert.Equal(2, item2.Currencies.Count);
        }


        [Theory]
        [MemberData(nameof(TestData_ContractCondition_Correct))]
        public void ContractConditionUpdater_Remove_Correct(
            Guid id,
            Guid companyId,
            decimal salaryMin,
            decimal salaryMax,
            int hoursPerTerm,
            bool isNegotiable,
            DateTime created,
            DateTime? removed,
            int? salaryTerm,
            int? currency,
            IEnumerable<int> workMode,
            IEnumerable<int> employmentType)
        {
            var builder = new ContractCondition.Builder()
                .SetId(id)
                .SetCompanyId(companyId)
                .SetSalaryRange(salaryMin, salaryMax)
                .SetHoursPerTerm(hoursPerTerm)
                .SetIsNegotiable(isNegotiable)
                .SetCreated(created)
                .SetRemoved(removed)
                .SetContractParameters(
                    (ContractAttributeInfo?)salaryTerm,
                    (ContractAttributeInfo?)currency,
                    workMode.Select(x => (ContractAttributeInfo?)x),
                    employmentType.Select(x => (ContractAttributeInfo?)x));

            Assert.False(builder.HasErrors());
            var item = builder.Build();

            Assert.False(item.Removed.HasValue);
            item.Remove();
            Assert.True(item.Removed.HasValue);
            item.Remove();
            Assert.False(item.Removed.HasValue);
        }
    }
}
