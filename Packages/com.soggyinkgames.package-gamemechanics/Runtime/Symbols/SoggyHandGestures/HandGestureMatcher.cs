using System;
using UnityEngine;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.Data;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SoggyHandGestures
{
    public sealed class HandGestureMatcher
    {
        private readonly GestureDefinition _definition;

        public HandGestureMatcher(GestureDefinition definition)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            if (definition.Symbol == null)
                throw new InvalidOperationException("GestureDefinition.Symbol is required.");

            if (string.IsNullOrWhiteSpace(definition.Symbol.Id))
                throw new InvalidOperationException("GestureDefinition.Symbol.Id is required.");

            if (definition.MaxDuration <= 0f)
                throw new InvalidOperationException("GestureDefinition.MaxDuration must be > 0.");

            if (definition.IsStatic)
            {
                if (definition.MaxMovementLength < 0f)
                    throw new InvalidOperationException("GestureDefinition.MaxMovementLength must be >= 0 for static gestures.");
            }
            else
            {
                if (definition.MaxDeviation <= 0f)
                    throw new InvalidOperationException("GestureDefinition.MaxDeviation must be > 0 for dynamic gestures.");

                if (definition.ReferencePaths == null || definition.ReferencePaths.Length == 0)
                    throw new InvalidOperationException("GestureDefinition.ReferencePaths is required for dynamic gestures.");

                for (int i = 0; i < definition.ReferencePaths.Length; i++)
                {
                    var path = definition.ReferencePaths[i];
                    if (path == null || path.Length < 2)
                        throw new InvalidOperationException($"GestureDefinition.ReferencePaths[{i}] must have at least 2 samples.");
                }
            }

            _definition = definition;
        }

        public SymbolMatch Match(HandGestureSample sample)
        {
            if (sample.Duration > _definition.MaxDuration)
                return default;

            if (_definition.IsStatic)
            {
                // Accept one sample as "no movement"
                if (sample.Path != null && sample.Path.Length == 1)
                    return new SymbolMatch(_definition.Symbol.Id, 1f);

                float movement = GesturePathMath.MovementLength(sample.Path);
                if (movement == float.MaxValue)
                    return default;

                float confidence = movement <= _definition.MaxMovementLength ? 1f : 0f;
                return new SymbolMatch(_definition.Symbol.Id, confidence);
            }

            float bestDeviation = float.MaxValue;

            for (int i = 0; i < _definition.ReferencePaths.Length; i++)
            {
                float d = GesturePathMath.Deviation(sample.Path, _definition.ReferencePaths[i]);
                if (d < bestDeviation) bestDeviation = d;
            }

            if (bestDeviation == float.MaxValue)
                return default;

            float finalConfidence = Mathf.Clamp01(1f - (bestDeviation / _definition.MaxDeviation));
            return new SymbolMatch(_definition.Symbol.Id, finalConfidence);
        }
    }
}