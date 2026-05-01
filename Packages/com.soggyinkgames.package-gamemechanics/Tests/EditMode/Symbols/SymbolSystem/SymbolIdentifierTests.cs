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

        [Test]
        public void Constructor_AssignsActionValues()
        {
            var identifier = new SymbolIdentifier(
                "ILLUMINATE",
                0.85f,
                SymbolSource.Gesture
            );

            Assert.AreEqual("ILLUMINATE", identifier.SymbolId);
            Assert.AreEqual(0.85f, identifier.Confidence);
            Assert.AreEqual(SymbolSource.Gesture, identifier.Source);
            Assert.Greater(identifier.Timestamp, 0f);
        }

        [Test]
        public void Timestamp_IncreasesOverTime()
        {
            var first = new SymbolIdentifier("A", 0.5f, SymbolSource.Gesture);
            var second = new SymbolIdentifier("A", 0.6f, SymbolSource.Gesture);

            Assert.GreaterOrEqual(second.Timestamp, first.Timestamp);
        }

        [Test]
        public void SupportsAllSources()
        {
            foreach (SymbolSource source in System.Enum.GetValues(typeof(SymbolSource)))
            {
                var id = new SymbolIdentifier("SRC", 0.5f, source);
                Assert.AreEqual(source, id.Source);
            }
        }
    }
    
}
