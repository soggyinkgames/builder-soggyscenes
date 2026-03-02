using NUnit.Framework;
using UnityEngine;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.Data;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Tests.Symbols.Data
{
    public class DataIntegrityTests
    {
        [Test]
        public void CanCreateSymbolDefinition()
        {
            var symbol = ScriptableObject.CreateInstance<SymbolDefinition>();
            symbol.Id = "DATA";

            Assert.AreEqual("DATA", symbol.Id);
        }

        [Test]
        public void CanCreateGestureDefinition()
        {
            var gesture = ScriptableObject.CreateInstance<GestureDefinition>();
            gesture.ReferencePath = TestPaths.Line();

            Assert.NotNull(gesture.ReferencePath);
        }
    }
    
}
