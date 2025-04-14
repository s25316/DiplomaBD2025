namespace Domain.Shared.ValueObjects.Ids
{
    public record SkillId
    {
        // Property
        public int Value { get; init; }


        // Constructor
        public SkillId(int value)
        {
            Value = value;
        }


        // Methods
        public static implicit operator SkillId(int value)
        {
            return new SkillId(value);
        }


        public static implicit operator int(SkillId value)
        {
            return value.Value;
        }
    }
}
