using Domain.Features.Offers.ValueObjects.Ids;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Features.Recruitments.Enums;
using Domain.Features.Recruitments.Exceptions;
using Domain.Features.Recruitments.ValueObjects.Ids;
using Domain.Shared.Enums;
using Domain.Shared.Templates;

namespace Domain.Features.Recruitments.Entities
{
    public partial class Recruitment : TemplateEntity<RecruitmentId>
    {
        // Properties
        public PersonId PersonId { get; private set; } = null!;
        public OfferId OfferId { get; private set; } = null!;
        public string? Message { get; private set; } = null;
        public string File { get; private set; } = null!;
        public DateTime Created { get; private set; }
        public ProcessType ProcessType { get; private set; } = ProcessType.Recruit;


        // Methods
        private void SetProcessType(ProcessType type)
        {
            if (((int)ProcessType > (int)type) ||
                ProcessType == ProcessType.Passed ||
                ProcessType == ProcessType.Rejected)
            {
                throw new RecruitmentException($"Unable change, actual: {ProcessType.Description()}")
            }
            ProcessType = type;
        }

        private void SetMessage(string? message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                Message = null;
            }
            else
            {
                Message = message;
            }
        }

        private void SetFile(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new RecruitmentException("File string can not be empty");
            }
            File = file;
        }
    }
}
