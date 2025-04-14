namespace Domain.Shared.Templates.Builders
{
    public abstract class TemplateUpdater<T>
        : TemplateBuilder<T> where T : class, new()
    {
        // Constructor
        protected TemplateUpdater(T value)
        {
            _value = value;
        }
    }
}
