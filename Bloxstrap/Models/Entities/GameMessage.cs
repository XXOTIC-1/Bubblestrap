namespace Bloxstrap.Models.Entities
{
    /// <summary>
    /// A [BloxstrapRPC] line emitted by a game. Only SetLaunchData is acted on now that rich
    /// presence is gone, since the invite deeplink still needs the launch data.
    /// </summary>
    public class GameMessage
    {
        [JsonPropertyName("command")]
        public string Command { get; set; } = null!;

        [JsonPropertyName("data")]
        public JsonElement Data { get; set; }
    }
}
