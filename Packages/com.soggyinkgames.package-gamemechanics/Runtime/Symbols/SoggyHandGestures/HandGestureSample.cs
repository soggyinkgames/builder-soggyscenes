using UnityEngine;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SoggyHandGestures
{
    public readonly struct HandGestureSample
    {
        public readonly Vector3[] Path;
        public readonly float Duration;

        public HandGestureSample(Vector3[] path, float duration)
        {
            Path = path;
            Duration = duration;
        }
    }

}