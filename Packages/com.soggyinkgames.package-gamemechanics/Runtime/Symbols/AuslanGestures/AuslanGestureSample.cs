using UnityEngine;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.AuslanGestures
{
    public readonly struct AuslanGestureSample
    {
        public readonly Vector3[] Path;
        public readonly float Duration;

        public AuslanGestureSample(Vector3[] path, float duration)
        {
            Path = path;
            Duration = duration;
        }
    }

}