using NUnit.Framework;
using UnityEngine;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.Data;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SoggyHandGestures;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SymbolsSystem;
using SoggyInkGames.Equanimous.PackageGameMechanics.Tests.Helpers;


namespace SoggyInkGames.Equanimous.PackageGameMechanics.Tests.Symbols.SymbolSystem
{
    public class SymbolSystemTests
    {
        private HandGestureDefinition CreateDefinition()
        {
            var def = ScriptableObject.CreateInstance<HandGestureDefinition>();
            def.IsStatic = false;
            def.ReferencePaths = new[] {TestPaths.Line()};
            def.MaxDeviation = 0.5f;
            def.MaxDuration = 2f;

            var symbol = ScriptableObject.CreateInstance<SymbolDefinition>();
            symbol.Id = "FLOW";
            def.Symbol = symbol;

            return def;
        }

        [Test]
        public void FullPipeline_RecognizesSymbol()
        {
            var def = CreateDefinition();
            var matcher = new HandGestureMatcher(def);
            var evaluator = new HandGestureEvaluator(0.7f);
            var system = new SymbolsSystem(new[] { matcher }, evaluator);

            bool recognized = false;

            SymbolEvents.SymbolRecognized += OnRecognized;

            void OnRecognized(SymbolIdentifier id)
            {
                Debug.Log($"RECOGNISED?: {id.SymbolId}");
                recognized = id.SymbolId == "FLOW";
            }

            var sample = new HandGestureSample(TestPaths.Line(), 1f);
            system.ProcessInput(sample);

            SymbolEvents.SymbolRecognized -= OnRecognized;

            Assert.IsTrue(recognized);
        }

        [Test]
        public void StaticSymbolWithMovement_WhenPathConfidenceBelowThreshold_FiresFailure()
        {
            var def = CreateDefinition();
            var matcher = new HandGestureMatcher(def);
            var evaluator = new HandGestureEvaluator(0.99f);
            var system = new SymbolsSystem(new[] { matcher }, evaluator);

            bool failed = false;

            SymbolEvents.SymbolFailed += OnFailed;

            void OnFailed(string reason)
            {
                failed = true;
            }

            var sample = new HandGestureSample(TestPaths.VShape(), 1f);
            Assert.That(matcher.Match(sample).Confidence, Is.LessThan(0.99f));

            system.ProcessInput(sample);

            SymbolEvents.SymbolFailed -= OnFailed;

            Assert.IsTrue(failed);
        }
    }
    
}
