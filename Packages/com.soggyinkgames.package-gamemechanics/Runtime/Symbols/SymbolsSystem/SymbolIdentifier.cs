using UnityEngine;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SymbolsSystem
{
    public enum SymbolSource
    {
        Gesture,
        Audio,
        Keyboard,
        Network
    }

    public readonly struct SymbolIdentifier
    {
        public readonly string SymbolId;
        public readonly float Confidence;
        public readonly SymbolSource Source;
        public readonly float Timestamp;

        public SymbolIdentifier(string symbolId, float confidence, SymbolSource source)
        {
            SymbolId = symbolId;
            Confidence = confidence;
            Source = source;
            Timestamp = Time.time;
        }
    }
}
