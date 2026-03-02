using NUnit.Framework;
using UnityEngine;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.Data;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.AuslanGestures;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Tests.Symbols.AuslanGestures
{
    public class AuslanGestureMatcherTests
    {
        private GestureDefinition CreateDefinition(Vector3[] path)
        {
            var def = ScriptableObject.CreateInstance<GestureDefinition>();
            def.ReferencePath = path;
            def.MaxDeviation = 0.5f;
            def.MaxDuration = 2f;

            var symbol = ScriptableObject.CreateInstance<SymbolDefinition>();
            symbol.Id = "TEST";
            def.Symbol = symbol;

            return def;
        }

        [Test]
        public void PerfectMatch_HighConfidence()
        {
            var def = CreateDefinition(TestPaths.Line());
            var matcher = new AuslanGestureMatcher(def);

            var sample = new AuslanGestureSample(TestPaths.Line(), 1f);
            var match = matcher.Match(sample);

            Assert.Greater(match.Confidence, 0.9f);
        }

        [Test]
        public void WrongShape_LowConfidence()
        {
            var def = CreateDefinition(TestPaths.Line());
            var matcher = new AuslanGestureMatcher(def);

            var sample = new AuslanGestureSample(TestPaths.VShape(), 1f);
            var match = matcher.Match(sample);

            Assert.Less(match.Confidence, 0.5f);
        }

        [Test]
        public void DurationTooLong_InvalidMatch()
        {
            var def = CreateDefinition(TestPaths.Line());
            def.MaxDuration = 0.5f;

            var matcher = new AuslanGestureMatcher(def);
            var sample = new AuslanGestureSample(TestPaths.Line(), 2f);

            var match = matcher.Match(sample);

            Assert.AreEqual(0f, match.Confidence);
        }
    }
    
}
