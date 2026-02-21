using UnityEngine;

namespace SoggyInkGames.Equanimous.PackageGameMechanics.Symbols.AuslanGestures
{
    public static class GestureMath
    {
        /// <summary>
        /// Calculates normalized deviation between two gesture paths.
        /// 0 = perfect match, higher = worse.
        /// </summary>
        public static float CalculateDeviation(Vector3[] sample, Vector3[] reference)
        {
            if (sample == null || reference == null)
                return float.MaxValue;

            if (sample.Length < 2 || reference.Length < 2)
                return float.MaxValue;

            // Resample both paths to equal resolution
            const int resolution = 32;

            var sampleResampled = Resample(sample, resolution);
            var referenceResampled = Resample(reference, resolution);

            // Normalize both paths
            Normalize(sampleResampled);
            Normalize(referenceResampled);

            float totalDistance = 0f;

            for (int i = 0; i < resolution; i++)
            {
                totalDistance += Vector3.Distance(
                    sampleResampled[i],
                    referenceResampled[i]
                );
            }

            return totalDistance / resolution;
        }

        // ----------------------------------------------------

        private static Vector3[] Resample(Vector3[] path, int targetCount)
        {
            Vector3[] result = new Vector3[targetCount];

            float totalLength = GetPathLength(path);
            float segmentLength = totalLength / (targetCount - 1);

            result[0] = path[0];

            float accumulated = 0f;
            int currentIndex = 1;

            for (int i = 1; i < path.Length; i++)
            {
                float distance = Vector3.Distance(path[i - 1], path[i]);

                while (accumulated + distance >= segmentLength)
                {
                    float t = (segmentLength - accumulated) / distance;
                    result[currentIndex++] =
                        Vector3.Lerp(path[i - 1], path[i], t);

                    distance -= (segmentLength - accumulated);
                    accumulated = 0f;

                    if (currentIndex >= targetCount)
                        return result;
                }

                accumulated += distance;
            }

            // Fill any remaining points
            for (int i = currentIndex; i < targetCount; i++)
                result[i] = path[path.Length - 1];

            return result;
        }

        private static float GetPathLength(Vector3[] path)
        {
            float length = 0f;

            for (int i = 1; i < path.Length; i++)
                length += Vector3.Distance(path[i - 1], path[i]);

            return length;
        }

        private static void Normalize(Vector3[] path)
        {
            // Center
            Vector3 center = Vector3.zero;
            foreach (var p in path)
                center += p;

            center /= path.Length;

            for (int i = 0; i < path.Length; i++)
                path[i] -= center;

            // Scale to unit size
            float maxMagnitude = 0f;

            foreach (var p in path)
                maxMagnitude = Mathf.Max(maxMagnitude, p.magnitude);

            if (maxMagnitude > 0f)
            {
                for (int i = 0; i < path.Length; i++)
                    path[i] /= maxMagnitude;
            }
        }
    }
}