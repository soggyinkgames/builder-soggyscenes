using System.Collections.Generic;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SoggyHandGestures;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SymbolsSystem
{
    public sealed class SymbolSystem
    {
        private readonly List<HandGestureMatcher> _gestureMatchers;
        private readonly HandGestureEvaluator _evaluator;

        public SymbolSystem(IEnumerable<HandGestureMatcher> matchers)
            : this(matchers, new HandGestureEvaluator(0.7f))
        {
        }

        public SymbolSystem(IEnumerable<HandGestureMatcher> matchers, HandGestureEvaluator evaluator)
        {
            _gestureMatchers = new List<HandGestureMatcher>(matchers);
            _evaluator = evaluator;
        }

        public void ProcessInput(HandGestureSample sample)
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
