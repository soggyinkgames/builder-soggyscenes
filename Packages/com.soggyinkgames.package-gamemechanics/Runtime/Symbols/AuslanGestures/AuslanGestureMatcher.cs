using UnityEngine;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.Data;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.AuslanGestures
{
    public sealed class AuslanGestureMatcher
    {
        private readonly GestureDefinition _definition;

        public AuslanGestureMatcher(GestureDefinition definition)
        {
            _definition = definition;
        }

        public SymbolMatch Match(AuslanGestureSample sample)
        {
            if (sample.Duration > _definition.MaxDuration)
                return default;

            float deviation = GestureMath.CalculateDeviation(
                sample.Path,
                _definition.ReferencePath
            );

            float confidence = 1f - (deviation / _definition.MaxDeviation);
            confidence = Mathf.Clamp01(confidence);

            return new SymbolMatch(
                _definition.Symbol.Id,
                confidence
            );
        }
    }
}
