namespace ActionCommandGame.Services.Helpers
{
    public class CumulativeGameEvent<T>
    {
        public required T GameEvent { get; set; }
        public int CumulativeProbability { get; set; }
    }
}
