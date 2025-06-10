using Domain.Features.OfferTemplates.Aggregates;
using Domain.Features.OfferTemplates.ValueObjects.Info;
using Domain.Shared.Enums;
using Domain.Shared.Exceptions;

namespace DomainTests
{
    public class AggregateOfferTemplateTests
    {
        // Tests
        public static IEnumerable<object[]> TestData_OfferTemplateBuilder_UnCorrect_TemplateBuilderException
        {
            get
            {
                yield return new object[]
                {
                    (Guid?) null,
                    (string?) null,
                    (string?) null,
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    (string?) null,
                    (string?) null,
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    " aa ",
                    (string?) null,
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_OfferTemplateBuilder_UnCorrect_TemplateBuilderException))]
        public void OfferTemplateBuilder_UnCorrect_TemplateBuilderException(
            Guid? companyId,
            string? name,
            string? description)
        {
            var builder = new OfferTemplate.Builder();

            if (companyId.HasValue)
            {
                builder.SetCompanyId(companyId.Value);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                builder.SetName(name);
            }
            if (!string.IsNullOrWhiteSpace(description))
            {
                builder.SetDescription(description);
            }

            var result = Assert.Throws<TemplateBuilderException>(
                () => builder.Build());
            Assert.Equal(HttpCode.InternalServerError, result.Code);
        }


        public static IEnumerable<object[]> TestData_OfferTemplateBuilder_Correct_Correct
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Name",
                    "Description",
                    (DateTime?) null,
                    (DateTime?) null,
                    (IEnumerable<int>)[1,2,3]
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Name",
                    "Description",
                    DateTime.Now,
                    (DateTime?) null,
                    (IEnumerable<int>)[1,2,3]
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_OfferTemplateBuilder_Correct_Correct))]
        public void OfferTemplateBuilder_Correct_Correct(
            Guid id,
            Guid companyId,
            string name,
            string description,
            DateTime? created,
            DateTime? removed,
            IEnumerable<int> skillIds)
        {
            var builder = new OfferTemplate.Builder()
                .SetId(id)
                .SetCompanyId(companyId)
                .SetName(name)
                .SetDescription(description)
                .SetRemoved(removed)
                .SetSkills(skillIds.Select(skillId => new OfferSkillInfo
                {
                    Created = null,
                    SkillId = skillId,
                    IsRequired = false,
                    Id = null,
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }

            Assert.False(builder.HasErrors());
            var item = builder.Build();
            Assert.NotNull(item.Created);
            Assert.Equal(skillIds.Count(), item.SkillsDictionary.Count);
        }


        public static IEnumerable<object[]> TestData_OfferTemplateBuilder_UnCorrect_HasErrors
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    " ",
                    "Description",
                    (DateTime?) null,
                    (DateTime?) null,
                    (IEnumerable<int>)[1,2,3]
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Name",
                    " ",
                    (DateTime?) null,
                    (DateTime?) null,
                    (IEnumerable<int>)[1,2,3]
                };

            }
        }
        [Theory]
        [MemberData(nameof(TestData_OfferTemplateBuilder_UnCorrect_HasErrors))]
        public void OfferTemplateBuilder_UnCorrect_HasErrors(
            Guid id,
            Guid companyId,
            string name,
            string description,
            DateTime? created,
            DateTime? removed,
            IEnumerable<int> skillIds)
        {
            var builder = new OfferTemplate.Builder()
                .SetId(id)
                .SetName(name)
                .SetCompanyId(companyId)
                .SetDescription(description)
                .SetRemoved(removed)
                .SetSkills(skillIds.Select(skillId => new OfferSkillInfo
                {
                    Created = null,
                    SkillId = skillId,
                    IsRequired = false,
                    Id = null,
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }

            Assert.True(builder.HasErrors());
        }


        public static IEnumerable<object[]> TestData_OfferTemplateBuilder_Remove_Correct
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Name",
                    "Description",
                    (DateTime?) null,
                    (DateTime?) null,
                    (IEnumerable<int>)[1,2,3]
                };

            }
        }
        [Theory]
        [MemberData(nameof(TestData_OfferTemplateBuilder_Remove_Correct))]
        public void OfferTemplateBuilder_Remove_Correct(
            Guid id,
            Guid companyId,
            string name,
            string description,
            DateTime? created,
            DateTime? removed,
            IEnumerable<int> skillIds)
        {
            var builder = new OfferTemplate.Builder()
                .SetId(id)
                .SetName(name)
                .SetCompanyId(companyId)
                .SetDescription(description)
                .SetRemoved(removed)
                .SetSkills(skillIds.Select(skillId => new OfferSkillInfo
                {
                    Created = null,
                    SkillId = skillId,
                    IsRequired = false,
                    Id = null,
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }

            Assert.False(builder.HasErrors());
            var item = builder.Build();

            Assert.False(item.Removed.HasValue);
            item.Remove();
            Assert.True(item.Removed.HasValue);
            item.Remove();
            Assert.False(item.Removed.HasValue);
        }


        public static IEnumerable<object[]> TestData_OfferTemplateUpdater_Update_Correct
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Name",
                    "Name2",
                    "Description",
                    "Description2",
                    (DateTime?) null,
                    (DateTime?) null,
                    (IEnumerable<int>)[1,2,3],
                    (IEnumerable<int>)[2,3,4]
                };

            }
        }
        [Theory]
        [MemberData(nameof(TestData_OfferTemplateUpdater_Update_Correct))]
        public void OfferTemplateUpdater_Update_Correct(
            Guid id,
            Guid companyId,
            string name,
            string name2,
            string description,
            string description2,
            DateTime? created,
            DateTime? removed,
            IEnumerable<int> skillIds,
            IEnumerable<int> skillIds2)
        {
            var builder = new OfferTemplate.Builder()
                .SetId(id)
                .SetName(name)
                .SetCompanyId(companyId)
                .SetDescription(description)
                .SetRemoved(removed)
                .SetSkills(skillIds.Select(skillId => new OfferSkillInfo
                {
                    Created = null,
                    SkillId = skillId,
                    IsRequired = false,
                    Id = null,
                }));

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }

            Assert.False(builder.HasErrors());
            var item = builder.Build();

            var updater = new OfferTemplate.Updater(item)
                .SetName(name2)
                .SetDescription(description2)
                .SetSkills(skillIds2.Select(skillId => new OfferSkillInfo
                {
                    Created = null,
                    SkillId = skillId,
                    IsRequired = false,
                    Id = null,
                }));

            Assert.False(updater.HasErrors());
            var item2 = updater.Build();

            Assert.Equal(name2, item2.Name);
            Assert.Equal(description2, item2.Description);

            var unionCount = skillIds.Union(skillIds2).Count();
            Assert.Equal(unionCount, item2.SkillsDictionary.Count);
        }
    }
}
