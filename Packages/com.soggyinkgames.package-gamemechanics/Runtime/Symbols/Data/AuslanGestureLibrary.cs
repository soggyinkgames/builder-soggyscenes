using UnityEngine;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.Data
{
    [CreateAssetMenu(menuName = "Symbols/Gesture Library")]
    public class GestureLibrary : ScriptableObject
    {
        public GestureDefinition[] Gestures;
    }
}
