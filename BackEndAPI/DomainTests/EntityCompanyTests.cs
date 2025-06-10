using Domain.Features.Companies.DomainEvents;
using Domain.Features.Companies.Entities;
using Domain.Shared.Enums;
using Domain.Shared.Exceptions;

namespace DomainTests
{
    public class EntityCompanyTests
    {
        // Properties
        public static IEnumerable<object[]> TestData_Company_CorrectBlocked
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    "Logo",
                    "Name",
                    "Description",
                    "1234567891",
                    "123456789",
                    "1234567891",
                    "https://github.com/s25316/DiplomaBD2025",
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                };
            }
        }
        public static IEnumerable<object[]> TestData_Company_CorrectUnBlocked
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    "Logo",
                    "Name",
                    "Description",
                    "1234567891",
                    "123456789",
                    "1234567891",
                    "https://github.com/s25316/DiplomaBD2025",
                    DateTime.Now,
                    null,
                    null,
                };
            }
        }


        // Tests
        [Fact]
        public void CompanyBuilder_UnCorrect_TemplateBuilderException()
        {
            var result = Assert.Throws<TemplateBuilderException>(
                () => new Company.Builder().Build());
            Assert.Equal(HttpCode.InternalServerError, result.Code);
        }


        public static IEnumerable<object[]> TestData_CompanyBuilder_Correct_Correct
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    "Logo",
                    "Name",
                    "Description",
                    "1234567891",
                    "123456789",
                    null,
                    "https://github.com/s25316/DiplomaBD2025",
                    null,
                    null,
                    null,
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    null,
                    "Name",
                    "Description",
                    "1234567891",
                    "123456789",
                    "1234567891",
                    null,
                    null,
                    null,
                    null,
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    null,
                    "Name",
                    " ",
                    "1234567891",
                    "123456789",
                    "1234567891",
                    (string?)null,
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    "Logo",
                    "Name",
                    "Description",
                    "1234567891",
                    "123456789",
                    "1234567891",
                    "https://github.com/s25316/DiplomaBD2025",
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_CompanyBuilder_Correct_Correct))]
        public void CompanyBuilder_Correct_Correct(
            Guid? id,
            string? logo,
            string name,
            string? description,
            string regon,
            string nip,
            string krs,
            string? url,
            DateTime? created,
            DateTime? removed,
            DateTime? blocked)
        {
            var builder = new Company.Builder()
                .SetName(name)
                .SetDescription(description)
                .SetRegon(regon)
                .SetNip(nip)
                .SetWebsiteUrl(url);

            if (id.HasValue)
            {
                builder.SetId(id.Value);
            }
            if (!string.IsNullOrWhiteSpace(logo))
            {
                builder.SetLogo(logo);
            }
            if (!string.IsNullOrWhiteSpace(krs))
            {
                builder.SetKrs(krs);
            }
            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }
            if (removed.HasValue)
            {
                builder.SetRemoved(removed.Value);
            }
            if (blocked.HasValue)
            {
                builder.SetBlocked(blocked.Value);
            }

            var item = builder.Build();
            Assert.Equal(name, item.Name);
            Assert.Equal(string.IsNullOrWhiteSpace(description), string.IsNullOrWhiteSpace(item.Description));
            Assert.Equal(regon, item.Regon);
            Assert.Equal(blocked, item.Blocked);
            Assert.Equal(removed, item.Removed);
            Assert.Equal(blocked.HasValue, item.IsBlocked);
            Assert.Empty(item.DomainEvents);
        }


        public static IEnumerable<object[]> TestData_CompanyBuilder_UnCorrect_HasErrors
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    "Logo",
                    " ",
                    "Description",
                    "1234567891",
                    "123456789",
                    "1234567891",
                    "https://github.com/s25316/DiplomaBD2025",
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    "Logo",
                    "Name",
                    "Description",
                    " ",
                    "123456789",
                    "1234567891",
                    "https://github.com/s25316/DiplomaBD2025",
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    "Logo",
                    "Name",
                    "Description",
                    "1234567891",
                    " ",
                    "1234567891",
                    "https://github.com/s25316/DiplomaBD2025",
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_CompanyBuilder_UnCorrect_HasErrors))]
        public void CompanyBuilder_UnCorrect_HasErrors(
            Guid? id,
            string? logo,
            string name,
            string? description,
            string regon,
            string nip,
            string krs,
            string? url,
            DateTime? created,
            DateTime? removed,
            DateTime? blocked)
        {
            var builder = new Company.Builder()
                .SetName(name)
                .SetDescription(description)
                .SetRegon(regon)
                .SetNip(nip)
                .SetWebsiteUrl(url);

            if (id.HasValue)
            {
                builder.SetId(id.Value);
            }
            if (!string.IsNullOrWhiteSpace(logo))
            {
                builder.SetLogo(logo);
            }
            if (!string.IsNullOrWhiteSpace(krs))
            {
                builder.SetKrs(krs);
            }
            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }
            if (removed.HasValue)
            {
                builder.SetRemoved(removed.Value);
            }
            if (blocked.HasValue)
            {
                builder.SetBlocked(blocked.Value);
            }

            Assert.True(builder.HasErrors());
        }


        [Theory]
        [MemberData(nameof(TestData_Company_CorrectBlocked))]
        public void CompanyUpdater_InputAgainKrs_HasErrors(
            Guid? id,
            string? logo,
            string name,
            string? description,
            string regon,
            string nip,
            string krs,
            string? url,
            DateTime created,
            DateTime? removed,
            DateTime? blocked)
        {
            var builder = new Company.Builder()
                .SetId(id.Value)
                .SetLogo(logo)
                .SetName(name)
                .SetDescription(description)
                .SetRegon(regon)
                .SetNip(nip)
                .SetKrs(krs)
                .SetWebsiteUrl(url)
                .SetCreated(created)
                .SetRemoved(removed)
                .SetBlocked(blocked);
            var item = builder.Build();

            var updater = new Company.Updater(item)
                .SetKrs(krs);

            Assert.True(updater.HasErrors());
            Assert.True((updater.GetErrors()?.Length ?? 0) > 0);
        }


        [Theory]
        [MemberData(nameof(TestData_Company_CorrectBlocked))]
        public void CompanyUpdater_Correct_Correct(
            Guid? id,
            string? logo,
            string name,
            string? description,
            string regon,
            string nip,
            string krs,
            string? url,
            DateTime created,
            DateTime? removed,
            DateTime? blocked)
        {
            var newDescription = "New Description";
            var newWWW = (string?)null;
            var newLogo = (string?)null;

            var builder = new Company.Builder()
                .SetId(id.Value)
                .SetLogo(logo)
                .SetName(name)
                .SetDescription(description)
                .SetRegon(regon)
                .SetNip(nip)
                .SetKrs(krs)
                .SetWebsiteUrl(url)
                .SetCreated(created)
                .SetRemoved(removed)
                .SetBlocked(blocked);
            var item = builder.Build();

            var updater = new Company.Updater(item)
                .SetDescription(newDescription)
                .SetWebsiteUrl(newWWW)
                .SetLogo(newLogo);

            Assert.False(updater.HasErrors());
            var newItem = updater.Build();

            Assert.Equal(newDescription, newItem.Description);
            Assert.Equal(newWWW, newItem.WebsiteUrl);
            Assert.Equal(newLogo, newItem.Logo);
        }


        [Theory]
        [MemberData(nameof(TestData_Company_CorrectBlocked))]
        public void CompanyUpdater_Correct_Unblock(
            Guid? id,
            string? logo,
            string name,
            string? description,
            string regon,
            string nip,
            string krs,
            string? url,
            DateTime created,
            DateTime? removed,
            DateTime? blocked)
        {
            var newDescription = "New Description";
            var newWWW = (string?)null;
            var newLogo = (string?)null;

            var builder = new Company.Builder()
                .SetId(id.Value)
                .SetLogo(logo)
                .SetName(name)
                .SetDescription(description)
                .SetRegon(regon)
                .SetNip(nip)
                .SetKrs(krs)
                .SetWebsiteUrl(url)
                .SetCreated(created)
                .SetRemoved(removed)
                .SetBlocked(blocked);
            var item = builder.Build();

            item.UnBlock();

            Assert.Null(item.Blocked);
            Assert.IsType<CompanyUnBlockedEvent>(item.DomainEvents.First());

            item.ClearEvents();
            Assert.Empty(item.DomainEvents);
        }


        [Theory]
        [MemberData(nameof(TestData_Company_CorrectUnBlocked))]
        public void CompanyUpdater_Correct_Block(
            Guid? id,
            string? logo,
            string name,
            string? description,
            string regon,
            string nip,
            string krs,
            string? url,
            DateTime created,
            DateTime? removed,
            DateTime? blocked)
        {
            var newDescription = "New Description";
            var newWWW = (string?)null;
            var newLogo = (string?)null;

            var builder = new Company.Builder()
                .SetId(id.Value)
                .SetLogo(logo)
                .SetName(name)
                .SetDescription(description)
                .SetRegon(regon)
                .SetNip(nip)
                .SetKrs(krs)
                .SetWebsiteUrl(url)
                .SetCreated(created)
                .SetRemoved(removed)
                .SetBlocked(blocked);
            var item = builder.Build();

            item.Block("Message");

            Assert.True(item.IsBlocked);
            Assert.IsType<CompanyBlockedEvent>(item.DomainEvents.First());

            item.ClearEvents();
            Assert.Empty(item.DomainEvents);
        }
    }
}
