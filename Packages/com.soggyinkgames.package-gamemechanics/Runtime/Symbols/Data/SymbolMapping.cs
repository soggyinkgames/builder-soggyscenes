using UnityEngine;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.Data
{
    [CreateAssetMenu(menuName = "Symbols/Symbol Mapping")]
    public class SymbolMapping : ScriptableObject
    {
        public HandGestureDefinition Gesture;
        public SymbolDefinition SymbolOverride;
    }
}
