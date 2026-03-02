using NUnit.Framework;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.AuslanGestures;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Tests.Symbols.AuslanGestures
{
    public class AuslanGestureEvaluatorTests
    {
        [Test]
        public void ConfidenceAboveThreshold_ReturnsTrue()
        {
            var evaluator = new AuslanGestureEvaluator(0.7f);
            var match = new SymbolMatch("A", 0.8f);

            Assert.IsTrue(evaluator.Evaluate(match));
        }

        [Test]
        public void ConfidenceBelowThreshold_ReturnsFalse()
        {
            var evaluator = new AuslanGestureEvaluator(0.7f);
            var match = new SymbolMatch("A", 0.5f);

            Assert.IsFalse(evaluator.Evaluate(match));
        }
    }
    
}