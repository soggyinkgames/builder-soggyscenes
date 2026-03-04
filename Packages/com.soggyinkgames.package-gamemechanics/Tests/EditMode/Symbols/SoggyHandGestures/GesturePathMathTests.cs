using NUnit.Framework;
using UnityEngine;
using SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SoggyHandGestures;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Tests.Symbols.SoggyHandGestures
{
    public class GesturePathMathTests
    {
        private const float Tolerance = 0.00001f;

        private static Vector3[] LineSamples(Vector3 start, Vector3 end, int sampleCount)
        {
            var samples = new Vector3[sampleCount];
            for (int i = 0; i < sampleCount; i++)
            {
                float t = i / (sampleCount - 1f);
                samples[i] = Vector3.Lerp(start, end, t);
            }
            return samples;
        }

        private static Vector3[] Translate(Vector3[] samples, Vector3 translation)
        {
            var r = new Vector3[samples.Length];
            for (int i = 0; i < samples.Length; i++) r[i] = samples[i] + translation;
            return r;
        }

        private static Vector3[] Scale(Vector3[] samples, float scale)
        {
            var r = new Vector3[samples.Length];
            for (int i = 0; i < samples.Length; i++) r[i] = samples[i] * scale;
            return r;
        }

        private static Vector3[] Reverse(Vector3[] samples)
        {
            var r = (Vector3[])samples.Clone();
            System.Array.Reverse(r);
            return r;
        }

        [Test]
        public void Deviation_ReturnsMaxValue_WhenInputsInvalid()
        {
            var valid = LineSamples(Vector3.zero, Vector3.right, 5);

            Assert.AreEqual(float.MaxValue, GesturePathMath.Deviation(null, valid));
            Assert.AreEqual(float.MaxValue, GesturePathMath.Deviation(valid, null));
            Assert.AreEqual(float.MaxValue, GesturePathMath.Deviation(new[] { Vector3.zero }, valid));
            Assert.AreEqual(float.MaxValue, GesturePathMath.Deviation(valid, new[] { Vector3.zero }));
        }

        [Test]
        public void Deviation_ReturnsZero_WhenPathsIdentical()
        {
            var a = new[]
            {
                new Vector3(0f,0f,0f),
                new Vector3(1f,0f,0f),
                new Vector3(2f,1f,0f),
                new Vector3(3f,1f,0f)
            };

            float d = GesturePathMath.Deviation((Vector3[])a.Clone(), a);

            Assert.That(d, Is.EqualTo(0f).Within(Tolerance));
        }

        [Test]
        public void Deviation_ReturnsZero_WhenTranslated()
        {
            var reference = new[]
            {
                new Vector3(0f,0f,0f),
                new Vector3(1f,0f,0f),
                new Vector3(2f,1f,0f),
                new Vector3(3f,1f,0f)
            };

            var recorded = Translate(reference, new Vector3(50f, -10f, 3f));

            float d = GesturePathMath.Deviation(recorded, reference);

            Assert.That(d, Is.EqualTo(0f).Within(Tolerance));
        }

        [Test]
        public void Deviation_ReturnsZero_WhenUniformlyScaled()
        {
            var reference = new[]
            {
                new Vector3(0f,0f,0f),
                new Vector3(1f,0f,0f),
                new Vector3(2f,1f,0f),
                new Vector3(3f,1f,0f)
            };

            var recorded = Scale(reference, 25f);

            float d = GesturePathMath.Deviation(recorded, reference);

            Assert.That(d, Is.EqualTo(0f).Within(Tolerance));
        }

        [Test]
        public void Deviation_ReturnsGreaterThanZero_WhenReversed()
        {
            var reference = new[]
            {
                new Vector3(0f,0f,0f),
                new Vector3(1f,0f,0f),
                new Vector3(2f,1f,0f),
                new Vector3(3f,1f,0f)
            };

            float d = GesturePathMath.Deviation(Reverse(reference), reference);

            Assert.That(d, Is.GreaterThan(0f));
        }

        [Test]
        public void MovementLength_ReturnsMaxValue_WhenInvalid()
        {
            Assert.AreEqual(float.MaxValue, GesturePathMath.MovementLength(null));
            Assert.AreEqual(float.MaxValue, GesturePathMath.MovementLength(new[] { Vector3.zero }));
        }

        [Test]
        public void MovementLength_ReturnsZero_WhenNoMovement()
        {
            var a = new[] { Vector3.one, Vector3.one };
            float m = GesturePathMath.MovementLength(a);

            Assert.That(m, Is.EqualTo(0f).Within(Tolerance));
        }

        [Test]
        public void MovementLength_ReturnsGreaterThanZero_WhenMovementExists()
        {
            var a = new[] { Vector3.zero, Vector3.right, Vector3.right * 2f };
            float m = GesturePathMath.MovementLength(a);

            Assert.That(m, Is.GreaterThan(0f));
        }
    }
}
