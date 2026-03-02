using NUnit.Framework;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SymbolsSystem;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Tests.Symbols.SymbolSystem
{
    public class SymbolIdentifierTests
    {
        [Test]
        public void Constructor_AssignsValues()
        {
            var identifier = new SymbolIdentifier(
                "A",
                0.85f,
                SymbolSource.Gesture
            );

            Assert.AreEqual("A", identifier.SymbolId);
            Assert.AreEqual(0.85f, identifier.Confidence);
            Assert.AreEqual(SymbolSource.Gesture, identifier.Source);
            Assert.Greater(identifier.Timestamp, 0f);
        }
    }
    
}
