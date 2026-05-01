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

        [Test]
        public void GestureLibraryStoresGestures()
        {
            var gesture = ScriptableObject.CreateInstance<GestureDefinition>();
            gesture.GestureId = "G1";

            var library = ScriptableObject.CreateInstance<GestureLibrary>();
            library.Gestures = new[] { gesture };

            Assert.AreEqual(1, library.Gestures.Length);
            Assert.AreEqual("G1", library.Gestures[0].GestureId);
        }

        [Test]
        public void SymbolMappingAllowsOverride()
        {
            var gesture = ScriptableObject.CreateInstance<GestureDefinition>();
            var symbol = ScriptableObject.CreateInstance<SymbolDefinition>();
            symbol.Id = "SYM";

            var mapping = ScriptableObject.CreateInstance<SymbolMapping>();
            mapping.Gesture = gesture;
            mapping.SymbolOverride = symbol;

            Assert.AreSame(gesture, mapping.Gesture);
            Assert.AreSame(symbol, mapping.SymbolOverride);
        }
    }
    
}
