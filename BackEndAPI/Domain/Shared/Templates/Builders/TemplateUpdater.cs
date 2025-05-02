namespace Domain.Shared.Templates.Builders
{
    public abstract class TemplateUpdater<T, TId>
        : TemplateBuilder<T, TId> where T : TemplateEntity<TId>, new()
    {
        // Constructor
        protected TemplateUpdater(T value)
        {
            _value = value;
        }
    }
}
