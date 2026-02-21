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

        private void OnSymbol(SymbolIdentifier token)
        {
            Debug.Log(
                $"[Symbol] {token.SymbolId} ({token.Confidence:0.00}) via {token.Source}"
            );
        }
    }
}


// namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.AuslanGestures
// namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.Data
// namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.Interfaces
// namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SymbolSystem
// namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.Utilities



