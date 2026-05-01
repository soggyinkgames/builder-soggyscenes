using NUnit.Framework;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SoggyHandGestures;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Tests.Symbols.SoggyHandGestures
{
    public class HandGestureEvaluatorTests
    {
        [Test]
        public void Evaluate_ReturnsTrue_WhenConfidenceAboveThreshold()
        {
            var evaluator = new HandGestureEvaluator(0.7f);
            var match = new SymbolMatch("A", 0.8f);

            Assert.IsTrue(evaluator.Evaluate(match));
        }

        [Test]
        public void Evaluate_ReturnsFalse_WhenConfidenceBelowThreshold()
        {
            var evaluator = new HandGestureEvaluator(0.7f);
            var match = new SymbolMatch("A", 0.5f);

            Assert.IsFalse(evaluator.Evaluate(match));
        }

        [Test]
        public void Evaluate_ReturnsTrue_WhenConfidenceEqualsThreshold()
        {
            var evaluator = new HandGestureEvaluator(0.7f);
            var match = new SymbolMatch("A", 0.7f);

            Assert.IsTrue(evaluator.Evaluate(match));
        }
    }
    
}
