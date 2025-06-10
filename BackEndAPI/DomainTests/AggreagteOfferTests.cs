using Domain.Features.Offers.Aggregates;
using Domain.Features.Offers.Enums;
using Domain.Features.Offers.Exceptions;
using Domain.Features.Offers.ValueObjects.Info;
using Domain.Shared.Enums;
using Domain.Shared.Exceptions;

namespace DomainTests
{
    public class AggreagteOfferTests
    {
        // Methods
        public static IEnumerable<object[]> TestData_OfferBuilder_Correct_Correct
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    (Guid?) null,
                    DateTime.Now.AddDays(-1),
                    (DateTime?) null,
                    (float?) null,
                    (string?) null,
                    (IEnumerable<Guid>) [],
                    OfferStatus.Active
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    DateTime.Now.AddDays(1),
                    (DateTime?) null,
                    (float?) 12,
                    "https://www.youtube.com/",
                    (IEnumerable<Guid>) [
                        Guid.NewGuid(),
                        Guid.NewGuid(),
                        ],
                    OfferStatus.Scheduled
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    DateTime.Now.AddDays(-1),
                    DateTime.Now.AddHours(-10),
                    (float?) 12,
                    "https://www.youtube.com/",
                    (IEnumerable<Guid>) [
                        Guid.NewGuid(),
                        Guid.NewGuid(),
                        ],
                    OfferStatus.Expired
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    DateTime.Now.AddDays(-1),
                    DateTime.Now.AddHours(10),
                    (float?) 12,
                    "https://www.youtube.com/",
                    (IEnumerable<Guid>) [
                        Guid.NewGuid(),
                        Guid.NewGuid(),
                        ],
                    OfferStatus.Active
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_OfferBuilder_Correct_Correct))]
        public void OfferBuilder_Correct_Correct(
            Guid id,
            Guid templateId,
            Guid? branchId,
            DateTime start,
            DateTime? end,
            float? employmentLength,
            string? url,
            IEnumerable<Guid> contractIds,
            OfferStatus status)
        {
            var builder = new Offer.Builder()
                .SetId(id)
                .SetOfferTemplate((TemplateInfo)templateId)
                .SetBranchId(branchId)
                .SetPublicationRange(start, end)
                .SetEmploymentLength(employmentLength)
                .SetWebsiteUrl(url)
                .SetContractConditions(contractIds.Select(x => (ContractInfo)x));

            Assert.False(builder.HasErrors());
            var item = builder.Build();

            Assert.Equal(contractIds.Count(), item.Contracts.Count());
            Assert.Equal(status, item.Status);
        }


        public static IEnumerable<object[]> TestData_OfferBuilder_UnCorrect_TemplateBuilderException
        {
            get
            {
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    (Guid?) null,
                    (DateTime?) null,
                    (DateTime?) null,
                    (float?) null,
                    (string?) null,
                    (IEnumerable<Guid>) []
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    (Guid?) null,
                    Guid.NewGuid(),
                    DateTime.Now,
                    (DateTime?) null,
                    (float?) 12,
                    "https://www.youtube.com/",
                    (IEnumerable<Guid>) [
                        Guid.NewGuid(),
                        Guid.NewGuid(),
                        ]
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_OfferBuilder_UnCorrect_TemplateBuilderException))]
        public void OfferBuilder_UnCorrect_TemplateBuilderException(
            Guid id,
            Guid? templateId,
            Guid? branchId,
            DateTime? start,
            DateTime? end,
            float? employmentLength,
            string? url,
            IEnumerable<Guid> contractIds)
        {
            var builder = new Offer.Builder()
                .SetId(id)
                .SetBranchId(branchId)
                .SetEmploymentLength(employmentLength)
                .SetWebsiteUrl(url)
                .SetContractConditions(contractIds.Select(x => (ContractInfo)x));
            if (templateId.HasValue)
            {
                builder.SetOfferTemplate((TemplateInfo)templateId);
            }
            if (start.HasValue)
            {
                builder.SetPublicationRange(start.Value, end);
            }

            var result = Assert.Throws<TemplateBuilderException>(
                () => builder.Build());
            Assert.Equal(HttpCode.InternalServerError, result.Code);
        }


        public static IEnumerable<object[]> TestData_OfferUpdater_Correct_Correct
        {
            get
            {
                var x1 = Guid.NewGuid();
                var x2 = Guid.NewGuid();
                var x3 = Guid.NewGuid();
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    DateTime.Now.AddDays(1),
                    DateTime.Now.AddDays(2),
                    (DateTime?) null,
                    DateTime.Now.AddDays(3),
                    (float?) 12,
                    (float?) 15,
                    "https://www.youtube.com/",
                    "https://pogoda.interia.pl/",
                    (IEnumerable<Guid>) [
                        x1,
                        x2,
                        ],
                    (IEnumerable<Guid>) [
                        x1,
                        x3,
                        ],
                    OfferStatus.Scheduled
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    DateTime.Now.AddDays(-1),
                    DateTime.Now.AddDays(2),
                    (DateTime?) null,
                    DateTime.Now.AddDays(3),
                    (float?) 12,
                    (float?) 15,
                    "https://www.youtube.com/",
                    "https://pogoda.interia.pl/",
                    (IEnumerable<Guid>) [
                        x1,
                        x2,
                        ],
                    (IEnumerable<Guid>) [
                        x1,
                        x3,
                        ],
                    OfferStatus.Active
                };
                yield return new object[]
                {
                    Guid.NewGuid(),
                    x1,
                    x1,
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    DateTime.Now.AddDays(-1),
                    DateTime.Now.AddDays(2),
                    (DateTime?) null,
                    DateTime.Now.AddDays(3),
                    (float?) 12,
                    (float?) 15,
                    "https://www.youtube.com/",
                    "https://pogoda.interia.pl/",
                    (IEnumerable<Guid>) [
                        x1,
                        x2,
                        ],
                    (IEnumerable<Guid>) [
                        x1,
                        x3,
                        ],
                    OfferStatus.Active
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_OfferUpdater_Correct_Correct))]
        public void OfferUpdater_Correct_Correct(
            Guid id,
            Guid templateId,
            Guid templateId2,
            Guid? branchId,
            Guid? branchId2,
            DateTime start,
            DateTime start2,
            DateTime? end,
            DateTime? end2,
            float? employmentLength,
            float? employmentLength2,
            string? url,
            string? url2,
            IEnumerable<Guid> contractIds,
            IEnumerable<Guid> contractIds2,
            OfferStatus offerStatus)
        {
            var builder = new Offer.Builder()
                .SetId(id)
                .SetOfferTemplate((TemplateInfo)templateId)
                .SetBranchId(branchId)
                .SetPublicationRange(start, end)
                .SetEmploymentLength(employmentLength)
                .SetWebsiteUrl(url)
                .SetContractConditions(contractIds.Select(x => (ContractInfo)x));

            Assert.False(builder.HasErrors());
            var item = builder.Build();

            var updater = new Offer.Updater(item)
                .SetOfferTemplate((TemplateInfo)templateId2)
                .SetBranchId(branchId2)
                .SetPublicationRange(start2, end2)
                .SetEmploymentLength(employmentLength2)
                .SetWebsiteUrl(url2)
                .SetContractConditions(contractIds2.Select(x => (ContractInfo)x));


            Assert.False(updater.HasErrors());
            var item2 = updater.Build();

            Assert.Equal(offerStatus, item2.Status);
            if (offerStatus == OfferStatus.Scheduled)
            {
                Assert.Equal(start2, item.PublicationRange.Start);
                Assert.Equal(end2, item.PublicationRange.End);
            }
            if (offerStatus == OfferStatus.Active)
            {
                Assert.Equal(start, item.PublicationRange.Start);
                Assert.Equal(end2, item.PublicationRange.End);
            }

            if (templateId != templateId2)
            {
                Assert.Equal(2, item2.Templates.Count);
            }
            else
            {
                Assert.Equal(1, item2.Templates.Count);
            }

            var expecated = contractIds.Except(contractIds2).Count();
            Assert.Equal(
                expecated,
                item2.Contracts.Values.Where(c => c.Removed.HasValue).Count());
        }


        public static IEnumerable<object[]> TestData_OfferUpdater_InvalidPublicationRange_HasErrors
        {
            get
            {
                yield return new object[]
                {
                    // Scheduled
                    DateTime.Now.AddDays(11),
                    DateTime.Now.AddDays(12),
                    DateTime.Now.AddDays(-12),
                    (DateTime?) null
                };
                yield return new object[]
                {
                    // Active
                    DateTime.Now.AddDays(-11),
                    DateTime.Now.AddDays(12),
                    DateTime.Now.AddDays(-12),
                    DateTime.Now.AddDays(-11),
                };
                yield return new object[]
                {
                    // Expired
                    DateTime.Now.AddDays(-11),
                    DateTime.Now.AddDays( -5),
                    DateTime.Now.AddDays(-12),
                    DateTime.Now.AddDays(-11),
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_OfferUpdater_InvalidPublicationRange_HasErrors))]
        public void OfferUpdater_InvalidPublicationRange_HasErrors(
            DateTime start,
            DateTime? end,
            DateTime start2,
            DateTime? end2)
        {
            var builder = new Offer.Builder()
                .SetId(Guid.NewGuid())
                .SetOfferTemplate((TemplateInfo)Guid.NewGuid())
                .SetBranchId(Guid.NewGuid())
                .SetPublicationRange(start, end)
                .SetEmploymentLength(12);

            Assert.False(builder.HasErrors());
            var item = builder.Build();

            var updater = new Offer.Updater(item)
                .SetPublicationRange(start2, end2);

            Assert.True(updater.HasErrors());
        }


        public static IEnumerable<object[]> TestData_Offer_Remove_Correct
        {
            get
            {
                yield return new object[]
                {
                    // Scheduled
                    DateTime.Now.AddDays(11),
                    DateTime.Now.AddDays(12),
                };
                yield return new object[]
                {
                    // Active
                    DateTime.Now.AddDays(-11),
                    DateTime.Now.AddDays(12),
                };
            }
        }
        [Theory]
        [MemberData(nameof(TestData_Offer_Remove_Correct))]
        public void Offer_Remove_Correct(
            DateTime start,
            DateTime? end)
        {
            var builder = new Offer.Builder()
                .SetId(Guid.NewGuid())
                .SetOfferTemplate((TemplateInfo)Guid.NewGuid())
                .SetBranchId(Guid.NewGuid())
                .SetPublicationRange(start, end)
                .SetEmploymentLength(12);

            Assert.False(builder.HasErrors());
            var item = builder.Build();
            item.Remove();

            Assert.Equal(OfferStatus.Expired, item.Status);
        }


        [Fact]
        public void Offer_Remove_OfferException()
        {
            var start = DateTime.Now.AddDays(-2);
            var end = DateTime.Now.AddDays(-1);

            var builder = new Offer.Builder()
                .SetId(Guid.NewGuid())
                .SetOfferTemplate((TemplateInfo)Guid.NewGuid())
                .SetBranchId(Guid.NewGuid())
                .SetPublicationRange(start, end)
                .SetEmploymentLength(12);

            Assert.False(builder.HasErrors());
            var item = builder.Build();
            Assert.Throws<OfferException>(() => item.Remove());
        }
    }
}
