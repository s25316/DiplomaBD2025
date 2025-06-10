using Domain.Features.Recruitments.Entities;
using Domain.Features.Recruitments.Enums;
using Domain.Shared.Enums;
using Domain.Shared.Exceptions;

namespace DomainTests
{
    public class EntityRecruitmentTests
    {
        // Properties


        // Methods
        public static IEnumerable<object[]> TestData_Branch_CorrectRemoved
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    (string?) null,
                    "file",
                    (DateTime?) DateTime.Now,
                    ProcessType.Recruit,
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    (string?) "",
                    "file",
                    (DateTime?) DateTime.Now,
                    ProcessType.Watched,
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    (string?) "aaa",
                    "file",
                    (DateTime?) DateTime.Now,
                    ProcessType.Recruit,
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_Branch_CorrectRemoved))]
        public void BranchBuilder_Correct_Correct(
            Guid id,
            Guid personId,
            Guid offerId,
            string? message,
            string file,
            DateTime? created,
            ProcessType process)
        {
            var builder = new Recruitment.Builder()
                .SetId(id)
                .SetPersonId(personId)
                .SetOfferId(offerId)
                .SetMessage(message)
                .SetFile(file)
                .SetProcessType(process);

            if (created.HasValue)
            {
                builder.SetCreated(created.Value);
            }

            Assert.False(builder.HasErrors());
            var item = builder.Build();

            Assert.Equal(process, item.ProcessType);
            Assert.Equal(file, item.File);
        }


        public static IEnumerable<object[]> TestData_BranchBuilder_UnCorrect_TemplateBuilderException
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    (Guid?) null,
                    Guid.NewGuid(),
                    (string?) null,
                    "file",
                    (DateTime?) DateTime.Now,
                    ProcessType.Recruit,
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    (Guid?) null,
                    (string?) "",
                    "file",
                    (DateTime?) DateTime.Now,
                    ProcessType.Watched,
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_BranchBuilder_UnCorrect_TemplateBuilderException))]
        public void BranchBuilder_UnCorrect_TemplateBuilderException(
            Guid id,
            Guid? personId,
            Guid? offerId,
            string? message,
            string file,
            DateTime? created,
            ProcessType process)
        {
            var builder = new Recruitment.Builder()
                .SetId(id)
                .SetMessage(message)
                .SetFile(file)
                .SetProcessType(process);

            if (personId.HasValue)
            {
                builder.SetPersonId(personId.Value);
            }
            if (offerId.HasValue)
            {
                builder.SetOfferId(offerId.Value);
            }
            var result = Assert.Throws<TemplateBuilderException>(
                () => builder.Build());
            Assert.Equal(HttpCode.InternalServerError, result.Code);
        }


        public static IEnumerable<object[]> TestData_BranchBuilder_UnCorrectFile_HasErrors
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    (string?) "",
                    " ",
                    (DateTime?) DateTime.Now,
                    ProcessType.Watched,
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    (string?) "",
                    " \n ",
                    (DateTime?) DateTime.Now,
                    ProcessType.Watched,
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_BranchBuilder_UnCorrectFile_HasErrors))]
        public void BranchBuilder_UnCorrectFile_HasErrors(
            Guid id,
            Guid personId,
            Guid offerId,
            string? message,
            string file,
            DateTime? created,
            ProcessType process)
        {
            var builder = new Recruitment.Builder()
                .SetId(id)
                .SetPersonId(personId)
                .SetOfferId(offerId)
                .SetMessage(message)
                .SetFile(file)
                .SetProcessType(process);

            Assert.True(builder.HasErrors());
        }


        public static IEnumerable<object[]> TestData_BranchBuilder_ChangingProcessType_Correct
        {
            get
            {
                yield return new object[]
                {
                    ProcessType.Recruit,
                    ProcessType.Watched,
                };
                yield return new object[]
                {
                    ProcessType.Watched,
                    ProcessType.Passed,
                };
                yield return new object[]
                {
                    ProcessType.Watched,
                    ProcessType.Rejected,
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_BranchBuilder_ChangingProcessType_Correct))]
        public void BranchBuilder_ChangingProcessType_Correct(
            ProcessType process1,
            ProcessType process2)
        {
            var builder = new Recruitment.Builder()
                .SetId(Guid.NewGuid())
                .SetPersonId(Guid.NewGuid())
                .SetOfferId(Guid.NewGuid())
                .SetMessage("xxx")
                .SetFile("xxx")
                .SetProcessType(process1);

            Assert.False(builder.HasErrors());
            var item = builder.Build();

            var updater = new Recruitment.Updater(item)
                .SetProcessType(process2);

            Assert.False(updater.HasErrors());
        }


        public static IEnumerable<object[]> TestData_BranchBuilder_ChangingProcessType_InCorrect
        {
            get
            {
                yield return new object[]
                {
                    ProcessType.Watched,
                    ProcessType.Recruit,
                    "XxxZ"
                };
                yield return new object[]
                {
                    ProcessType.Passed,
                    ProcessType.Watched,
                    "XxxA"
                };
                yield return new object[]
                {
                    ProcessType.Rejected,
                    ProcessType.Watched,
                    "XxxB"
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_BranchBuilder_ChangingProcessType_InCorrect))]
        public void BranchBuilder_ChangingProcessType_InCorrect(
            ProcessType process1,
            ProcessType process2,
            string file)
        {
            var builder = new Recruitment.Builder()
                .SetId(Guid.NewGuid())
                .SetPersonId(Guid.NewGuid())
                .SetOfferId(Guid.NewGuid())
                .SetMessage("xxx")
                .SetFile("xxx")
                .SetProcessType(process1);

            Assert.False(builder.HasErrors());
            var item = builder.Build();

            var updater = new Recruitment.Updater(item)
                .SetProcessType(process2)
                .SetFile(file);

            Assert.True(updater.HasErrors());
            var item2 = updater.Build();
            Assert.Equal(file, item2.File);
        }
    }
}
