using UnityEngine;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.Data
{
    [CreateAssetMenu(menuName = "Symbols/Gesture Definition")]
    public class GestureDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string GestureId;
        public SymbolDefinition Symbol;

        [Header("Timing")]
        public float MaxDuration = 1.5f;

        [Header("Movement")]
        [Tooltip("If true, movement matching is treated as static (no movement expected).")]
        public bool IsStatic = true;

        [Tooltip("Used only when IsStatic is true. Total movement length must be <= this value.")]
        public float MaxMovementLength = 0.02f;

        [Tooltip("Used only when IsStatic is false. Deviation must be <= this value.")]
        public float MaxDeviation = 0.15f;

        [Tooltip("Used only when IsStatic is false. One or more reference paths (variants).")]
        public Vector3[][] ReferencePaths;
    }
}