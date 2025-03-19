using Domain.Shared.Exceptions;
using System.Text;

namespace Domain.Shared.Templates
{
    public abstract class TemplateBuilder<T> where T : class, new()
    {
        //Properties
        private readonly T _value = new T();
        private readonly StringBuilder _errors = new StringBuilder();


        // Public Methods
        public bool HasErrors() => _errors.Length > 0;
        public string? GetErrors() => _errors.Length == 0 ? null : _errors.ToString().Trim();
        public T Build()
        {
            SetDefaultValues()(_value);
            // IF INPUT DATA HAS NO ERRORS CHECH IS OBJECT COMPLETE
            if (!HasErrors())
            {
                var incompleteData = CheckIsObjectComplete()(_value);
                if (!string.IsNullOrWhiteSpace(incompleteData))
                {
                    var message = new StringBuilder();
                    message.Append(Messages.TemplateBuilder_IncompleteBuilding);
                    message.Append($", of class {nameof(T)}, left data: ");
                    message.AppendLine(incompleteData);
                    throw new TemplateBuilderException(message.ToString());
                }
            }
            return _value;
        }

        // Protected Methods
        protected abstract Action<T> SetDefaultValues();
        protected abstract Func<T, string> CheckIsObjectComplete();
        protected TemplateBuilder<T> SetProperty(Action<T> propertySetter)
        {
            try
            {
                propertySetter(_value);
            }
            catch (Exception ex)
            {
                AppendError(ex.Message);
            }
            return this;
        }

        // Private Methods
        private void AppendError(string error) => _errors.AppendLine(error);
    }
}
