
namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SoggyHandGestures
{
    public readonly struct SymbolMatch
    {
        public readonly string SymbolId;
        public readonly float Confidence;

        public SymbolMatch(string symbolId, float confidence)
        {
            SymbolId = symbolId;
            Confidence = confidence;
        }
    }
}