namespace GameEventModule.Domain
{
    /// <summary>
    /// ID định danh duy nhất của Game Event
    /// </summary>
    public readonly struct GameEventId
    {
        public readonly string Value;

        public GameEventId(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}