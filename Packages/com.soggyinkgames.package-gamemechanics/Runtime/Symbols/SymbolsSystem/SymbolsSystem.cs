using System.Collections.Generic;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.AuslanGestures;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SymbolsSystem
{
    public sealed class SymbolSystem
    {
        private readonly List<AuslanGestureMatcher> _gestureMatchers;
        private readonly AuslanGestureEvaluator _evaluator;

        public SymbolSystem(IEnumerable<AuslanGestureMatcher> matchers)
            : this(matchers, new AuslanGestureEvaluator(0.7f))
        {
        }

        public SymbolSystem(IEnumerable<AuslanGestureMatcher> matchers, AuslanGestureEvaluator evaluator)
        {
            _gestureMatchers = new List<AuslanGestureMatcher>(matchers);
            _evaluator = evaluator;
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

            bool meetsThreshold = _evaluator?.Evaluate(bestMatch) ?? bestScore > 0.7f;

            if (meetsThreshold)
            {
                var identity = new SymbolIdentifier(
                    bestMatch.SymbolId,
                    bestScore,
                    SymbolSource.Gesture
                );

                SymbolEvents.RaiseRecognized(identity);
            }
            else
            {
                SymbolEvents.RaiseFailed("No confident symbol match");
            }
        }
    }
}
