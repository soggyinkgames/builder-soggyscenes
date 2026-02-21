using System.Collections.Generic;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.AuslanGestures;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SymbolsSystem
{
    public sealed class SymbolSystem
    {
        private readonly List<AuslanGestureMatcher> _gestureMatchers;

        public SymbolSystem(IEnumerable<AuslanGestureMatcher> matchers)
        {
            _gestureMatchers = new List<AuslanGestureMatcher>(matchers);
        }

        public void ProcessInput(AuslanGestureSample sample)
        {
            SymbolMatch bestMatch = default;
            float bestScore = 0f;

            foreach (var matcher in _gestureMatchers)
            {
                var match = matcher.Match(sample);
                if (match.Confidence > bestScore)
                {
                    bestScore = match.Confidence;
                    bestMatch = match;
                }
            }

            if (bestScore > 0.7f)
            {
                var token = new SymbolIdentifier(
                    bestMatch.SymbolId,
                    bestScore,
                    SymbolSource.Gesture
                );

                SymbolEvents.RaiseRecognized(token);
            }
            else
            {
                SymbolEvents.RaiseFailed("No confident symbol match");
            }
        }
    }
}
