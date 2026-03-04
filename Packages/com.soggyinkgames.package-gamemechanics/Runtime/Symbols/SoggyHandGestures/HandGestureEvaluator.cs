namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SoggyHandGestures
{
    /// <summary>
    /// Evaluates whether a gesture match meets the required confidence. Can add difficulty curves
    /// </summary>
    public sealed class HandGestureEvaluator
    {
        private readonly float _requiredConfidence;

        public HandGestureEvaluator(float requiredConfidence)
        {
            _requiredConfidence = requiredConfidence;
        }

        public bool Evaluate(SymbolMatch match)
        {
            return match.Confidence >= _requiredConfidence;
        }
    }
}
