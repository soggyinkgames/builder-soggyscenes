using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SymbolsSystem;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.Interfaces
{
    public interface ISymbolListener
    {
        void OnSymbol(SymbolIdentifier identity);
    }
}
