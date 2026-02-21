namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.AuslanGestures
{
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
