using System;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SymbolsSystem
{
    public static class SymbolEvents
    {
        public static event Action<SymbolIdentifier> SymbolRecognized;
        public static event Action<string> SymbolFailed;

        public static void RaiseRecognized(SymbolIdentifier identity)
        {
            SymbolRecognized?.Invoke(identity);
        }

        public static void RaiseFailed(string reason)
        {
            SymbolFailed?.Invoke(reason);
        }
    }
}
