using UnityEngine;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.Data
{
    [CreateAssetMenu(menuName = "Symbols/Gesture Definition")]
    public class GestureDefinition : ScriptableObject
    {
        public string GestureId;
        public SymbolDefinition Symbol;

        public Vector3[] ReferencePath;
        public float MaxDeviation = 0.15f;
        public float MaxDuration = 1.5f;
    }
}
