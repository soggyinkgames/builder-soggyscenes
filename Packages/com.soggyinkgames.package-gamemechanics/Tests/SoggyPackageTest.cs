namespace SoggyInkGames.SoggyPackage.Tests
{
    using NUnit.Framework;
    using UnityEngine;

    public class SoggyPackageTestTests
    {
        [Test]
        public void SoggyPackageTestSimplePasses()
        {
            // Use the Assert class to test conditions
            Assert.IsTrue(true);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode, it will simply run to completion.
        // [UnityTest]
        // public IEnumerator SoggyPackageTestWithEnumeratorPasses()
        // {
        //     // Use the Assert class to test conditions.
        //     // Use yield to skip a frame.
        //     yield return null;
        // }
    }
}