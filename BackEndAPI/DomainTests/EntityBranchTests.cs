using Domain.Features.Branches.Entities;
using Domain.Shared.Enums;
using Domain.Shared.Exceptions;

namespace DomainTests
{
    public class EntityBranchTests
    {
        // Properties
        public static IEnumerable<object[]> TestData_Branch_CorrectRemoved
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Name",
                    "Description",
                    DateTime.Now,
                    DateTime.Now,
                };
            }
        }


        // Methods
        public static IEnumerable<object[]> TestData_BranchBuilder_Correct_Correct
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Name",
                    "Description",
                    DateTime.Now,
                    DateTime.Now,
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Name",
                    " ",
                    (DateTime?)null,
                    DateTime.Now,
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_BranchBuilder_Correct_Correct))]
        public void BranchBuilder_Correct_Correct(
            Guid id,
            Guid companyId,
            Guid addressId,
            string name,
            string? description,
            DateTime? created,
            DateTime? removed)
        {
            var builder = new Branch.Builder()
                .SetId(id)
                .SetCompanyId(companyId)
                .SetAddressId(addressId)
                .SetName(name)
                .SetDescription(description)
                .SetRemoved(removed);

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }

            Assert.False(builder.HasErrors());
            var item = builder.Build();

            Assert.Equal(name, item.Name);
            Assert.NotNull(item.Created);
        }


        [Theory]
        [MemberData(nameof(TestData_BranchBuilder_Correct_Correct))]
        public void BranchBuilder_EmptyName_HasErrors(
           Guid id,
           Guid companyId,
           Guid addressId,
           string name,
           string? description,
           DateTime? created,
           DateTime? removed)
        {
            var builder = new Branch.Builder()
                .SetId(id)
                .SetCompanyId(companyId)
                .SetAddressId(addressId)
                .SetName(" ")
                .SetDescription(description)
                .SetRemoved(removed);

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }

            Assert.True(builder.HasErrors());
        }


        [Theory]
        [MemberData(nameof(TestData_Branch_CorrectRemoved))]
        public void BranchBuilder_Remove_Correct(
           Guid id,
           Guid companyId,
           Guid addressId,
           string name,
           string? description,
           DateTime? created,
           DateTime? removed)
        {
            var builder = new Branch.Builder()
                .SetId(id)
                .SetCompanyId(companyId)
                .SetAddressId(addressId)
                .SetName(name)
                .SetDescription(description)
                .SetRemoved(removed);

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }

            Assert.False(builder.HasErrors());
            var item = builder.Build();

            Assert.True(item.Removed.HasValue);
            item.Remove();
            Assert.False(item.Removed.HasValue);
        }


        public static IEnumerable<object[]> TestData_BranchBuilder_UnCorrect_TemplateBuilderException
        {
            get
            {
                yield return new object[]
                {
                    (Guid?) null,
                    (Guid?) null,
                    (string?) null,
                };
                yield return new object[]
                {
                    (Guid?) Guid.NewGuid(),
                    (Guid?) null,
                    (string?) null,
                };
                yield return new object[]
                {
                    (Guid?) Guid.NewGuid(),
                    (Guid?) Guid.NewGuid(),
                    (string?) null,
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_BranchBuilder_UnCorrect_TemplateBuilderException))]
        public void BranchBuilder_UnCorrect_TemplateBuilderException(
           Guid? companyId,
           Guid? addressId,
           string? name)
        {
            var builder = new Branch.Builder();

            if (companyId.HasValue)
            {
                builder.SetCompanyId(companyId.Value);
            }
            if (addressId.HasValue)
            {
                builder.SetAddressId(addressId.Value);
            }
            if (name != null)
            {
                builder.SetName(name);
            }

            var result = Assert.Throws<TemplateBuilderException>(
                () => builder.Build());
            Assert.Equal(HttpCode.InternalServerError, result.Code);
        }


        [Theory]
        [MemberData(nameof(TestData_Branch_CorrectRemoved))]
        public void BranchUpdater_Correct_Correct(
            Guid id,
            Guid companyId,
            Guid addressId,
            string name,
            string? description,
            DateTime created,
            DateTime? removed)
        {
            var builder = new Branch.Builder()
                .SetId(id)
                .SetCompanyId(companyId)
                .SetAddressId(Guid.NewGuid())
                .SetName("Name")
                .SetDescription(null)
                .SetCreated(created)
                .SetRemoved(removed);
            Assert.False(builder.HasErrors());

            var item = builder.Build();
            var updater = new Branch.Updater(item)
                .SetAddressId(addressId)
                .SetName(name)
                .SetDescription(description);
            Assert.False(updater.HasErrors());
            var item2 = updater.Build();

            Assert.Equal(addressId, (Guid)item2.AddressId);
            Assert.Equal(name, item2.Name);
            Assert.Equal(description, item2.Description);
        }
    }
}
