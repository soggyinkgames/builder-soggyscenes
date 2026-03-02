using UnityEngine;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SymbolsSystem;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.Utilities
{
    public sealed class SymbolDebug : MonoBehaviour
    {
        private void OnEnable()
        {
            SymbolEvents.SymbolRecognized += OnSymbol;
        }

        private void OnDisable()
        {
            SymbolEvents.SymbolRecognized -= OnSymbol;
        }

        private void OnSymbol(SymbolIdentifier identity)
        {
            Debug.Log(
                $"[Symbol] {identity.SymbolId} ({identity.Confidence:0.00}) via {identity.Source}"
            );
        }
    }
}

