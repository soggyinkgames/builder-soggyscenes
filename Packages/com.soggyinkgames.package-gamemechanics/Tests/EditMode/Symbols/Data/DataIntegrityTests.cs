using NUnit.Framework;
using UnityEngine;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.Data;
using SoggyInkGames.Equanimous.PackageGameMechanics.Tests.Helpers;


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
            var gesture = ScriptableObject.CreateInstance<HandGestureDefinition>();
            gesture.ReferencePaths = new[] { TestPaths.Line() };

            Assert.NotNull(gesture.ReferencePaths);
        }

        [Test]
        public void HandGestureDefinition_MaintainsDataIntegrity()
        {
            // 1. Setup
            var gesture = ScriptableObject.CreateInstance<HandGestureDefinition>();
            var symbol = ScriptableObject.CreateInstance<SymbolDefinition>();
            symbol.Id = "FLOW";

            // 2. Assign values
            gesture.Symbol = symbol;
            gesture.GestureId = "Flow_LeftToRight";
            gesture.IsStatic = false;
            gesture.MaxDuration = 2.0f;
            gesture.ReferencePaths = new[] { TestPaths.Line() };

            // 3. Assertions (Check the whole "contract")
            Assert.AreEqual("FLOW", gesture.Symbol.Id, "Symbol link failed.");
            Assert.IsFalse(gesture.IsStatic);
            Assert.AreEqual(2.0f, gesture.MaxDuration);
            Assert.AreEqual(1, gesture.ReferencePaths.Length);
            Assert.AreEqual(3, gesture.ReferencePaths[0].Length); // Ensure the path has transforms path references
        }

        [Test]
        public void GestureLibraryStoresGestures()
        {
            var gesture = ScriptableObject.CreateInstance<HandGestureDefinition>();
            gesture.GestureId = "G1";

            var library = ScriptableObject.CreateInstance<GestureLibrary>();
            library.Gestures = new[] { gesture };

            Assert.AreEqual(1, library.Gestures.Length);
            Assert.AreEqual("G1", library.Gestures[0].GestureId);
        }

        [Test]
        public void SymbolMappingAllowsOverride()
        {
            var gesture = ScriptableObject.CreateInstance<HandGestureDefinition>();
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
