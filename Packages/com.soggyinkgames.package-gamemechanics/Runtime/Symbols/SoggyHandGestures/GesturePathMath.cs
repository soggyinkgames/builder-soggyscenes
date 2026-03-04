using UnityEngine;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.SoggyHandGestures
{
    public static class GesturePathMath
    {
        private const int ResampleCount = 32;

        public static bool IsValidPath(Vector3[] path)
        {
            return path != null && path.Length >= 2;
        }

        /// <summary>
        /// Average distance between two paths after resample + normalize.
        /// 0 = identical, higher = worse.
        /// </summary>
        public static float Deviation(Vector3[] recordedPath, Vector3[] referencePath)
        {
            if (!IsValidPath(recordedPath) || !IsValidPath(referencePath))
                return float.MaxValue;

            var a = Resample(recordedPath, ResampleCount);
            var b = Resample(referencePath, ResampleCount);

            NormalizeInPlace(a);
            NormalizeInPlace(b);

            float total = 0f;
            for (int i = 0; i < ResampleCount; i++)
                total += Vector3.Distance(a[i], b[i]);

            return total / ResampleCount;
        }

        /// <summary>
        /// Total movement length in world units.
        /// Returns float.MaxValue for null or length < 2.
        /// </summary>
        public static float MovementLength(Vector3[] recordedPath)
        {
            if (!IsValidPath(recordedPath))
                return float.MaxValue;

            float length = 0f;
            for (int i = 1; i < recordedPath.Length; i++)
                length += Vector3.Distance(recordedPath[i - 1], recordedPath[i]);

            return length;
        }

        // -------------------- helpers --------------------

        private static Vector3[] Resample(Vector3[] path, int sampleCount)
        {
            var result = new Vector3[sampleCount];

            float totalLength = PathLength(path);
            float stepLength = totalLength / (sampleCount - 1);

            result[0] = path[0];

            float carried = 0f;
            int write = 1;

            for (int i = 1; i < path.Length; i++)
            {
                float segmentLength = Vector3.Distance(path[i - 1], path[i]);

                while (carried + segmentLength >= stepLength)
                {
                    float t = stepLength <= 0f ? 0f : (stepLength - carried) / segmentLength;
                    result[write++] = Vector3.Lerp(path[i - 1], path[i], t);

                    segmentLength -= (stepLength - carried);
                    carried = 0f;

                    if (write >= sampleCount)
                        return result;
                }

                carried += segmentLength;
            }

            for (int i = write; i < sampleCount; i++)
                result[i] = path[path.Length - 1];

            return result;
        }

        private static float PathLength(Vector3[] path)
        {
            float length = 0f;
            for (int i = 1; i < path.Length; i++)
                length += Vector3.Distance(path[i - 1], path[i]);
            return length;
        }

        private static void NormalizeInPlace(Vector3[] samples)
        {
            Vector3 center = Vector3.zero;
            for (int i = 0; i < samples.Length; i++)
                center += samples[i];
            center /= samples.Length;

            for (int i = 0; i < samples.Length; i++)
                samples[i] -= center;

            float maxMagnitude = 0f;
            for (int i = 0; i < samples.Length; i++)
                maxMagnitude = Mathf.Max(maxMagnitude, samples[i].magnitude);

            if (maxMagnitude > 0f)
            {
                for (int i = 0; i < samples.Length; i++)
                    samples[i] /= maxMagnitude;
            }
        }
    }
}