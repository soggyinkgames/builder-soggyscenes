namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.AuslanGestures
{
    /// <summary>
    /// Evaluates whether a gesture match meets the required confidence. Can add difficulty curves
    /// </summary>
    public sealed class AuslanGestureEvaluator
    {
        private readonly float _requiredConfidence;

        public AuslanGestureEvaluator(float requiredConfidence)
        {
            _requiredConfidence = requiredConfidence;
        }

        public bool Evaluate(SymbolMatch match)
        {
            return match.Confidence >= _requiredConfidence;
        }
    }
}
