using NUnit.Framework;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SymbolsSystem;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Tests.Symbols.SymbolSystem
{
    public class SymbolEventsTests
    {
        [Test]
        public void RaiseRecognized_InvokesSubscriber()
        {
            bool called = false;

            SymbolEvents.SymbolRecognized += OnRecognized;

            void OnRecognized(SymbolIdentifier id)
            {
                called = true;
            }

            SymbolEvents.RaiseRecognized(
                new SymbolIdentifier("A", 1f, SymbolSource.Gesture)
            );

            SymbolEvents.SymbolRecognized -= OnRecognized;

            Assert.IsTrue(called);
        }

        [Test]
        public void RaiseFailed_InvokesSubscriber()
        {
            bool called = false;

            SymbolEvents.SymbolFailed += OnFailed;

            void OnFailed(string reason)
            {
                called = true;
            }

            SymbolEvents.RaiseFailed("test failure");

            SymbolEvents.SymbolFailed -= OnFailed;

            Assert.IsTrue(called);
        }
    }
    
}
