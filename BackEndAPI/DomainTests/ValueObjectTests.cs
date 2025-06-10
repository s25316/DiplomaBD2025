using Domain.Features.Companies.ValueObjects.Krss;
using Domain.Features.Companies.ValueObjects.Nips;
using Domain.Features.Companies.ValueObjects.Regons;
using Domain.Features.ContractConditions.ValueObjects.HoursPerTerms;
using Domain.Features.ContractConditions.ValueObjects.Info;
using Domain.Features.ContractConditions.ValueObjects.Moneys;
using Domain.Features.ContractConditions.ValueObjects.SalaryRanges;
using Domain.Features.Offers.ValueObjects;
using Domain.Features.Offers.ValueObjects.EmploymentLengths;
using Domain.Features.Offers.ValueObjects.Info;
using Domain.Features.People.ValueObjects.BirthDates;
using Domain.Features.People.ValueObjects.Info;
using Domain.Features.People.ValueObjects.PhoneNumbers;
using Domain.Shared.Enums;
using Domain.Shared.ValueObjects.Emails;
using Domain.Shared.ValueObjects.Urls;

namespace DomainTests
{
    public class ValueObjectTests
    {
        // Methods
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("a@gmail.com")]
        [InlineData("a@pjwtsk.edu.pl")]
        public void Email_Correct_Correct(string? value)
        {
            var item = (Email?)(value);
            if (string.IsNullOrWhiteSpace(value))
            {
                Assert.True(item == null);
                Assert.Null((string?)item);
            }
            else
            {
                Assert.Equal(value, (string?)item);
            }
        }


        [Theory]
        [InlineData("agmail.com")]
        [InlineData("a/pjwtsk.edu.pl")]
        public void Email_UnCorrect_EmailException(string value)
        {
            var ex = Assert.Throws<EmailException>(() => (Email?)(value));
            Assert.Equal(HttpCode.BadRequest, ex.Code);
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("https://www.youtube.com/")]
        [InlineData("https://www.wp.pl/")]
        [InlineData("http://github.com/s25316/DiplomaBD2025")]
        public void UrlProperty_Correct_Correct(string? value)
        {
            var item = (UrlProperty?)(value);
            if (string.IsNullOrWhiteSpace(value))
            {
                Assert.True(item == null);
                Assert.Null((string?)item);
            }
            else
            {
                Assert.Equal(value, (string?)item);
                Assert.Equal(value, item?.ToString());
            }
        }


        [Theory]
        [InlineData("agmail.com")]
        [InlineData("a/pjwtsk.edu.pl")]
        public void UrlProperty_UnCorrect_UrlPropertyException(string value)
        {
            var ex = Assert.Throws<UrlPropertyException>(() => (UrlProperty?)(value));
            Assert.Equal(HttpCode.BadRequest, ex.Code);
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("1234567891")]
        [InlineData("1234-567-891")]
        [InlineData("123 456 789 1")]
        public void Krs_Correct_Correct(string? value)
        {
            var item = (Krs?)(value);
            if (string.IsNullOrWhiteSpace(value))
            {
                Assert.True(item == null);
                Assert.Null((string?)item);
            }
            else
            {
                value = value
                .Replace("-", "")
                .Replace(" ", "")
                .Trim();
                Assert.Equal(value, (string?)item);
                Assert.Equal(value, item?.ToString());
            }
        }


        [Theory]
        [InlineData("123456789")]
        [InlineData("12345678912")]
        [InlineData("agmail.com")]
        [InlineData("a/pjwtsk.edu.pl")]
        public void Krs_UnCorrect_KrsException(string value)
        {
            var ex = Assert.Throws<KrsException>(() => (Krs?)(value));
            Assert.Equal(HttpCode.BadRequest, ex.Code);
        }


        [Theory]
        [InlineData("123456789")]
        [InlineData("123-456-789")]
        [InlineData("123 456 789 ")]
        public void Nip_Correct_Correct(string value)
        {
            var item = (Nip)(value);
            value = value
                .Replace("-", "")
                .Replace(" ", "")
                .Trim();
            Assert.Equal(value, (string?)item);
            Assert.Equal(value, item?.ToString());
        }


        [Theory]
        [InlineData("12345678")]
        [InlineData("12345678912")]
        [InlineData("agmail.com")]
        [InlineData("a/pjwtsk.edu.pl")]
        public void Nip_UnCorrect_NipException(string value)
        {
            var ex = Assert.Throws<NipException>(() => (Nip?)(value));
            Assert.Equal(HttpCode.BadRequest, ex.Code);
        }


        [Theory]
        [InlineData("1234567891")]
        [InlineData("123-456-7892")]
        [InlineData("123 456 7893 ")]
        public void Regon_Correct_Correct(string value)
        {
            var item = (Regon)(value);
            value = value
                .Replace("-", "")
                .Replace(" ", "")
                .Trim();
            Assert.Equal(value, (string?)item);
            Assert.Equal(value, item?.ToString());
        }


        [Theory]
        [InlineData("12345678")]
        [InlineData("12345678912")]
        [InlineData("agmail.com")]
        [InlineData("a/pjwtsk.edu.pl")]
        public void Regon_UnCorrect_RegonException(string value)
        {
            var ex = Assert.Throws<RegonException>(() => (Regon)(value));
            Assert.Equal(HttpCode.BadRequest, ex.Code);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(111)]
        [InlineData(11100)]
        public void HoursPerTerm_Correct_Correct(int value)
        {
            var item = (HoursPerTerm)(value);
            Assert.Equal(value, (int)item);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void HoursPerTerm_UnCorrect_HoursPerTermException(int value)
        {
            var ex = Assert.Throws<HoursPerTermException>(() => (HoursPerTerm)(value));
            Assert.Equal(HttpCode.BadRequest, ex.Code);
        }


        [Theory]
        [InlineData(null)]
        [InlineData(1)]
        [InlineData(111)]
        [InlineData(11100)]
        public void ContractAttributeInfo_CorrectNullAble_Correct(int? value)
        {
            var item = (ContractAttributeInfo?)(value);
            Assert.Equal(value, item?.ContractParameterId);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(111)]
        [InlineData(11100)]
        public void ContractAttributeInfo_CorrectNotNullAble_Correct(int value)
        {
            var item = (ContractAttributeInfo?)(value);
            Assert.Equal(value, item?.ContractParameterId);
        }


        [Theory]
        [InlineData(120)]
        [InlineData(120.11)]
        [InlineData(120.1)]
        public void Money_Correct_Correct(decimal value)
        {
            var item = (Money)(value);
            Assert.Equal(value, (decimal)item);
        }


        [Theory]
        [InlineData(-0.1)]
        [InlineData(120.111)]
        [InlineData(-120.1)]
        public void Money_UnCorrect_MoneyException(decimal value)
        {
            var ex = Assert.Throws<MoneyException>(() => (Money)(value));
            Assert.Equal(HttpCode.BadRequest, ex.Code);
        }


        [Theory]
        [InlineData(0, 120)]
        [InlineData(120.11, 0)]
        [InlineData(11, 12)]
        public void SalaryRange_Correct_Correct(decimal value1, decimal value2)
        {
            var item = new SalaryRange(value1, value2);
            if (value1 < value2)
            {
                Assert.Equal(value1, (decimal)item.Min);
                Assert.Equal(value2, (decimal)item.Max);
            }
            else
            {

                Assert.Equal(value2, (decimal)item.Min);
                Assert.Equal(value1, (decimal)item.Max);
            }
        }


        [Theory]
        [InlineData(-1, 1)]
        [InlineData(-11, -12)]
        [InlineData(-120.1, -1)]
        public void SalaryRange_UnCorrect_SalaryRangeException(decimal value1, decimal value2)
        {
            var ex = Assert.Throws<SalaryRangeException>(() => new SalaryRange(value1, value2));
            Assert.Equal(HttpCode.BadRequest, ex.Code);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(120.11)]
        [InlineData(120.1)]
        public void EmploymentLength_Correct_Correct(float value)
        {
            var item = (EmploymentLength)(value);
            Assert.Equal(value, (float)item);
        }


        [Theory]
        [InlineData(-0.1)]
        [InlineData(-120.111)]
        [InlineData(-120.1)]
        public void EmploymentLength_UnCorrect_EmploymentLengthException(float value)
        {
            var ex = Assert.Throws<EmploymentLengthException>(() => new EmploymentLength(value));
            Assert.Equal(HttpCode.BadRequest, ex.Code);
        }


        [Fact]
        public void ContractInfo_CorrectGuid_Correct()
        {
            // Assert
            var value = Guid.NewGuid();
            // Act 
            var item = (ContractInfo)value;

            Assert.Equal(value, item.ContractConditionId);
            Assert.Null(item.Created);
        }


        [Fact]
        public void TemplateInfo_CorrectGuid_Correct()
        {
            // Assert
            var value = Guid.NewGuid();
            // Act 
            var item = (TemplateInfo)value;

            Assert.Equal(value, item.OfferTemplateId);
            Assert.Null(item.Created);
        }


        public static IEnumerable<object[]> TestData_PublicationRange_Correct_Correct
        {
            get
            {
                DateTime now = DateTime.Now;
                yield return new object[] { now, now };
                yield return new object[] { now.AddDays(-1), now.AddDays(1) };
                yield return new object[] { now.AddDays(1), now.AddDays(-1) };
                yield return new object[] { now.AddDays(1), (DateTime?)null };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_PublicationRange_Correct_Correct))]
        public void PublicationRange_Correct_Correct(DateTime value1, DateTime? value2)
        {
            var item = new PublicationRange(value1, value2);
            if (value2 == null)
            {
                Assert.Equal(value1, item.Start);
                Assert.Equal(value2, item.End);
            }
            else if (value2 < value1)
            {
                Assert.Equal(value2, item.Start);
                Assert.Equal(value1, item.End);
            }
            else
            {
                Assert.Equal(value1, item.Start);
                Assert.Equal(value2, item.End);
            }
        }


        public static IEnumerable<object[]> TestData_BirthDate_CorrectNullAbleDateOnly_Correct
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                yield return new object[] { today.AddDays(-1) };
                yield return new object[] { today.AddMonths(-1) };
                yield return new object[] { today.AddYears(-1) };
                yield return new object[] { (DateOnly?)null };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_BirthDate_CorrectNullAbleDateOnly_Correct))]
        public void BirthDate_CorrectNullAbleDateOnly_Correct(DateOnly? value)
        {
            var item = (BirthDate?)(value);
            Assert.Equal(value, (DateOnly?)item);
        }

        public static IEnumerable<object[]> TestData_BirthDate_CorrectNotNullAbleDateOnly_Correct
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                yield return new object[] { today.AddDays(-1) };
                yield return new object[] { today.AddMonths(-1) };
                yield return new object[] { today.AddYears(-1) };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_BirthDate_CorrectNotNullAbleDateOnly_Correct))]
        public void BirthDate_CorrectNotNullAbleDateOnly_Correct(DateOnly value)
        {
            var item = (BirthDate)(value);
            Assert.Equal(value, (DateOnly)item);
        }

        public static IEnumerable<object[]> TestData_BirthDate_UnCorrect_BirthDateException
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                yield return new object[] { today.AddDays(1) };
                yield return new object[] { today.AddMonths(1) };
                yield return new object[] { today.AddYears(1) };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_BirthDate_UnCorrect_BirthDateException))]
        public void BirthDate_UnCorrect_BirthDateException(DateOnly value)
        {
            var ex = Assert.Throws<BirthDateException>(() => (BirthDate)(value));
            Assert.Equal(HttpCode.BadRequest, ex.Code);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(111)]
        [InlineData(11100)]
        public void PersonSkillInfo_CorrectNotNullAble_Correct(int value)
        {
            var item = (PersonSkillInfo)(value);
            Assert.Equal(value, item.SkillId);
            Assert.NotNull(item.Created);
        }


        [Theory]
        [InlineData(1, "a")]
        [InlineData(111, "b")]
        [InlineData(11100, "c")]
        public void PersonUrlInfo_CorrectNotNullAble_Correct(int value1, string value2)
        {
            var item = PersonUrlInfo.Prepare(value1, value2);
            Assert.Equal(value1, item.UrlTypeId);
            Assert.Equal(value2, item.Value);
            Assert.NotNull(item.Created);
            Assert.Null(item.Removed);
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("48 123 456 789")]
        [InlineData("123 456 789")]
        [InlineData("3 456 789")]
        public void PhoneNumber_Correct_Correct(string? value)
        {
            var item = (PhoneNumber?)(value);
            if (string.IsNullOrWhiteSpace(value))
            {
                Assert.True(item == null);
                Assert.Null((string?)item);
            }
            else
            {
                value = value
                    .Replace("-", "")
                    .Replace("\n", "")
                    .Replace("\t", "")
                    .Replace(" ", "");
                Assert.Equal(value, (string?)item);
            }
        }


        [Theory]
        [InlineData("agmail.com")]
        [InlineData("a/pjwtsk.edu.pl")]
        public void PhoneNumber_UnCorrect_PhoneNumberException(string value)
        {
            var ex = Assert.Throws<PhoneNumberException>(() => (PhoneNumber?)(value));
            Assert.Equal(HttpCode.BadRequest, ex.Code);
        }
    }
}