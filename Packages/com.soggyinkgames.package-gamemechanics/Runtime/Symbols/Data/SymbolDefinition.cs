using UnityEngine;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.Data
{
    [CreateAssetMenu(menuName = "Symbols/Symbol Definition")]
    public class SymbolDefinition : ScriptableObject
    {
        public string Id;
        public string DisplayName;
        [TextArea] public string Description;

        public Color SymbolColor;
        public AudioClip AudioCue;

        [Range(0, 5)]
        public int Difficulty;
    }
}
