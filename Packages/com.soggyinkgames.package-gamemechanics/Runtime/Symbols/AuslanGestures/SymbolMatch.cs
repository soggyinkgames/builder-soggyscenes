
namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.AuslanGestures
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