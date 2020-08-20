using JetBrains.ReSharper.Feature.Services.Daemon;

namespace CleanCode
{
    [RegisterConfigurableHighlightingsGroup(CleanCode, "Clean Code")]
    public static class CleanCodeHighlightingGroupIds
    {
        public const string CleanCode = "CleanCode";
    }
}