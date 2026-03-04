using System;
using NUnit.Framework;
using UnityEngine;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.Data;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SoggyHandGestures;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Tests.Symbols.SoggyHandGestures
{
    public class HandGestureMatcherTests
    {
        private static GestureDefinition CreateBaseDefinition()
        {
            var def = ScriptableObject.CreateInstance<GestureDefinition>();
            def.GestureId = "TEST_GESTURE";
            def.Symbol = new SymbolDefinition { Id = "TEST" };
            def.MaxDuration = 1f;
            return def;
        }

        private static Vector3[] TwoSamplePath(Vector3 a, Vector3 b) => new[] { a, b };

        private static HandGestureSample Sample(float duration, Vector3[] path)
            => new HandGestureSample { Duration = duration, Path = path };

        [Test]
        public void Constructor_ThrowsArgumentNullException_WhenDefinitionNull()
        {
            Assert.Throws<ArgumentNullException>(() => new HandGestureMatcher(null));
        }

        [Test]
        public void Constructor_ThrowsInvalidOperationException_WhenSymbolMissing()
        {
            var def = ScriptableObject.CreateInstance<GestureDefinition>();
            def.MaxDuration = 1f;
            def.IsStatic = true;
            def.MaxMovementLength = 0.01f;

            Assert.Throws<InvalidOperationException>(() => new HandGestureMatcher(def));
            UnityEngine.Object.DestroyImmediate(def);
        }

        [Test]
        public void Constructor_ThrowsInvalidOperationException_WhenSymbolIdMissing()
        {
            var def = CreateBaseDefinition();
            def.Symbol = new SymbolDefinition { Id = "" };
            def.IsStatic = true;
            def.MaxMovementLength = 0.01f;

            Assert.Throws<InvalidOperationException>(() => new HandGestureMatcher(def));
            UnityEngine.Object.DestroyImmediate(def);
        }

        [Test]
        public void Constructor_ThrowsInvalidOperationException_WhenMaxDurationNotPositive()
        {
            var def = CreateBaseDefinition();
            def.MaxDuration = 0f;
            def.IsStatic = true;
            def.MaxMovementLength = 0.01f;

            Assert.Throws<InvalidOperationException>(() => new HandGestureMatcher(def));
            UnityEngine.Object.DestroyImmediate(def);
        }

        [Test]
        public void Constructor_ThrowsInvalidOperationException_WhenStaticMaxMovementLengthNegative()
        {
            var def = CreateBaseDefinition();
            def.IsStatic = true;
            def.MaxMovementLength = -0.01f;

            Assert.Throws<InvalidOperationException>(() => new HandGestureMatcher(def));
            UnityEngine.Object.DestroyImmediate(def);
        }

        [Test]
        public void Constructor_ThrowsInvalidOperationException_WhenDynamicMaxDeviationNotPositive()
        {
            var def = CreateBaseDefinition();
            def.IsStatic = false;
            def.MaxDeviation = 0f;
            def.ReferencePaths = new[] { TwoSamplePath(Vector3.zero, Vector3.right) };

            Assert.Throws<InvalidOperationException>(() => new HandGestureMatcher(def));
            UnityEngine.Object.DestroyImmediate(def);
        }

        [Test]
        public void Constructor_ThrowsInvalidOperationException_WhenDynamicReferencePathsNull()
        {
            var def = CreateBaseDefinition();
            def.IsStatic = false;
            def.MaxDeviation = 1f;
            def.ReferencePaths = null;

            Assert.Throws<InvalidOperationException>(() => new HandGestureMatcher(def));
            UnityEngine.Object.DestroyImmediate(def);
        }

        [Test]
        public void Constructor_ThrowsInvalidOperationException_WhenDynamicReferencePathsEmpty()
        {
            var def = CreateBaseDefinition();
            def.IsStatic = false;
            def.MaxDeviation = 1f;
            def.ReferencePaths = Array.Empty<Vector3[]>();

            Assert.Throws<InvalidOperationException>(() => new HandGestureMatcher(def));
            UnityEngine.Object.DestroyImmediate(def);
        }

        [Test]
        public void Constructor_ThrowsInvalidOperationException_WhenAnyDynamicReferencePathTooShort()
        {
            var def = CreateBaseDefinition();
            def.IsStatic = false;
            def.MaxDeviation = 1f;
            def.ReferencePaths = new[] { new[] { Vector3.zero } };

            Assert.Throws<InvalidOperationException>(() => new HandGestureMatcher(def));
            UnityEngine.Object.DestroyImmediate(def);
        }

        [Test]
        public void Match_ReturnsDefault_WhenDurationExceedsLimit()
        {
            var def = CreateBaseDefinition();
            def.IsStatic = false;
            def.MaxDeviation = 1f;
            def.ReferencePaths = new[] { TwoSamplePath(Vector3.zero, Vector3.right) };

            var matcher = new HandGestureMatcher(def);

            var result = matcher.Match(Sample(2f, TwoSamplePath(Vector3.zero, Vector3.right)));

            Assert.AreEqual(default(SymbolMatch), result);
            UnityEngine.Object.DestroyImmediate(def);
        }

        [Test]
        public void Match_Static_ReturnsDefault_WhenPathNull()
        {
            var def = CreateBaseDefinition();
            def.IsStatic = true;
            def.MaxMovementLength = 0.01f;

            var matcher = new HandGestureMatcher(def);

            var result = matcher.Match(Sample(0.5f, null));

            Assert.AreEqual(default(SymbolMatch), result);
            UnityEngine.Object.DestroyImmediate(def);
        }

        [Test]
        public void Match_Static_ReturnsConfidenceOne_WhenPathHasOneSample()
        {
            var def = CreateBaseDefinition();
            def.IsStatic = true;
            def.MaxMovementLength = 0f;

            var matcher = new HandGestureMatcher(def);

            var result = matcher.Match(Sample(0.5f, new[] { Vector3.one }));

            Assert.AreEqual("TEST", result.SymbolId);
            Assert.AreEqual(1f, result.Confidence);
            UnityEngine.Object.DestroyImmediate(def);
        }

        [Test]
        public void Match_Static_ReturnsConfidenceOne_WhenMovementWithinLimit()
        {
            var def = CreateBaseDefinition();
            def.IsStatic = true;
            def.MaxMovementLength = 0.2f;

            var matcher = new HandGestureMatcher(def);

            var result = matcher.Match(Sample(0.5f, TwoSamplePath(Vector3.zero, Vector3.right * 0.1f)));

            Assert.AreEqual("TEST", result.SymbolId);
            Assert.AreEqual(1f, result.Confidence);
            UnityEngine.Object.DestroyImmediate(def);
        }

        [Test]
        public void Match_Static_ReturnsConfidenceZero_WhenMovementOverLimit()
        {
            var def = CreateBaseDefinition();
            def.IsStatic = true;
            def.MaxMovementLength = 0.01f;

            var matcher = new HandGestureMatcher(def);

            var result = matcher.Match(Sample(0.5f, TwoSamplePath(Vector3.zero, Vector3.right)));

            Assert.AreEqual("TEST", result.SymbolId);
            Assert.AreEqual(0f, result.Confidence);
            UnityEngine.Object.DestroyImmediate(def);
        }

        [Test]
        public void Match_Dynamic_ReturnsDefault_WhenRecordedPathNull()
        {
            var def = CreateBaseDefinition();
            def.IsStatic = false;
            def.MaxDeviation = 1f;
            def.ReferencePaths = new[] { TwoSamplePath(Vector3.zero, Vector3.right) };

            var matcher = new HandGestureMatcher(def);

            var result = matcher.Match(Sample(0.5f, null));

            Assert.AreEqual(default(SymbolMatch), result);
            UnityEngine.Object.DestroyImmediate(def);
        }

        [Test]
        public void Match_Dynamic_ReturnsDefault_WhenRecordedPathTooShort()
        {
            var def = CreateBaseDefinition();
            def.IsStatic = false;
            def.MaxDeviation = 1f;
            def.ReferencePaths = new[] { TwoSamplePath(Vector3.zero, Vector3.right) };

            var matcher = new HandGestureMatcher(def);

            var result = matcher.Match(Sample(0.5f, new[] { Vector3.zero }));

            Assert.AreEqual(default(SymbolMatch), result);
            UnityEngine.Object.DestroyImmediate(def);
        }

        [Test]
        public void Match_Dynamic_ReturnsConfidenceOne_WhenRecordedMatchesAReferencePath()
        {
            var def = CreateBaseDefinition();
            def.IsStatic = false;
            def.MaxDeviation = 1f;
            def.ReferencePaths = new[] { TwoSamplePath(Vector3.zero, Vector3.right) };

            var matcher = new HandGestureMatcher(def);

            var result = matcher.Match(Sample(0.5f, TwoSamplePath(Vector3.zero, Vector3.right)));

            Assert.AreEqual("TEST", result.SymbolId);
            Assert.AreEqual(1f, result.Confidence);
            UnityEngine.Object.DestroyImmediate(def);
        }

        [Test]
        public void Match_Dynamic_ReturnsConfidenceZero_WhenDeviationExceedsMaxDeviation()
        {
            var def = CreateBaseDefinition();
            def.IsStatic = false;
            def.MaxDeviation = 0.01f;
            def.ReferencePaths = new[] { TwoSamplePath(Vector3.zero, Vector3.right) };

            var matcher = new HandGestureMatcher(def);

            var result = matcher.Match(Sample(0.5f, TwoSamplePath(Vector3.zero, Vector3.up)));

            Assert.AreEqual("TEST", result.SymbolId);
            Assert.AreEqual(0f, result.Confidence);
            UnityEngine.Object.DestroyImmediate(def);
        }

        [Test]
        public void Match_Dynamic_UsesBestOfMultipleReferencePaths()
        {
            var def = CreateBaseDefinition();
            def.IsStatic = false;
            def.MaxDeviation = 1f;

            var good = TwoSamplePath(Vector3.zero, Vector3.right);
            var bad = TwoSamplePath(Vector3.zero, Vector3.up);

            def.ReferencePaths = new[] { bad, good };

            var matcher = new HandGestureMatcher(def);

            var result = matcher.Match(Sample(0.5f, good));

            Assert.AreEqual("TEST", result.SymbolId);
            Assert.AreEqual(1f, result.Confidence);
            UnityEngine.Object.DestroyImmediate(def);
        }
    }
}