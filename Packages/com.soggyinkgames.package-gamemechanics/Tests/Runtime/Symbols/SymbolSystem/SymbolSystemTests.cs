using NUnit.Framework;
using UnityEngine;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.Data;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SymbolsSystem;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.AuslanGestures;
using RuntimeSymbolSystem = SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SymbolsSystem.SymbolSystem;



namespace SoggyInkGames.Equanimous.PackageGameMechanics.Tests.Symbols.SymbolSystem
{
    public class SymbolSystemTests
    {
        private GestureDefinition CreateDefinition()
        {
            var def = ScriptableObject.CreateInstance<GestureDefinition>();
            def.ReferencePath = TestPaths.Line();
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
            var matcher = new AuslanGestureMatcher(def);
            var evaluator = new AuslanGestureEvaluator(0.7f);
            var system = new RuntimeSymbolSystem(new[] { matcher }, evaluator);

            bool recognized = false;

            SymbolEvents.SymbolRecognized += OnRecognized;

            void OnRecognized(SymbolIdentifier id)
            {
                recognized = id.SymbolId == "FLOW";
            }

            var sample = new AuslanGestureSample(TestPaths.Line(), 1f);
            system.ProcessInput(sample);

            SymbolEvents.SymbolRecognized -= OnRecognized;

            Assert.IsTrue(recognized);
        }

        [Test]
        public void BelowThreshold_FiresFailure()
        {
            var def = CreateDefinition();
            var matcher = new AuslanGestureMatcher(def);
            var evaluator = new AuslanGestureEvaluator(0.99f);
            var system = new RuntimeSymbolSystem(new[] { matcher }, evaluator);

            bool failed = false;

            SymbolEvents.SymbolFailed += OnFailed;

            void OnFailed(string reason)
            {
                failed = true;
            }

            var sample = new AuslanGestureSample(TestPaths.SlightlyOffsetLine(), 1f);
            system.ProcessInput(sample);

            SymbolEvents.SymbolFailed -= OnFailed;

            Assert.IsTrue(failed);
        }
    }
    
}
