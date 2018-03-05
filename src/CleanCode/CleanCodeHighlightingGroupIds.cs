using CleanCode;
using JetBrains.ReSharper.Feature.Services.Daemon;

[assembly: RegisterConfigurableHighlightingsGroup(CleanCodeHighlightingGroupIds.CleanCode, "Clean Code")]

namespace CleanCode
{
    public static class CleanCodeHighlightingGroupIds
    {
        public const string CleanCode = "CleanCode";
    }
}