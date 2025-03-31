namespace ActionCommandGame.Services.Model.Results
{
    public class BuyResult
    {
        public PlayerResult Player { get; set; } = null!;
        public ItemResult Item { get; set; } = null!;
    }
}
