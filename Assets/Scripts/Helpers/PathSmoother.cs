using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public class PathSmoother
    {
        public Vector3[] SmoothPath(Vector3[] pathPoints, int subdivisions = 10)
        {
            int numPoints = pathPoints.Length;
            int smoothedPointsCount = (numPoints - 1) * subdivisions + 1;
            Vector3[] smoothedPath = new Vector3[smoothedPointsCount];

            for (int i = 0; i < numPoints - 1; i++)
            {
                for (int j = 0; j < subdivisions; j++)
                {
                    float t = j / (float)subdivisions;
                    smoothedPath[i * subdivisions + j] = CatmullRomInterpolation(
                        pathPoints[ClampIndex(i - 1, numPoints)],
                        pathPoints[i],
                        pathPoints[ClampIndex(i + 1, numPoints)],
                        pathPoints[ClampIndex(i + 2, numPoints)],
                        t
                    );
                }
            }

            smoothedPath[smoothedPointsCount - 1] = pathPoints[numPoints - 1];

            return smoothedPath;
        }

        private Vector3 CatmullRomInterpolation(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            return 0.5f * (
                2.0f * p1 +
                (-p0 + p2) * t +
                (2.0f * p0 - 5.0f * p1 + 4.0f * p2 - p3) * t * t +
                (-p0 + 3.0f * p1 - 3.0f * p2 + p3) * t * t * t
            );
        }

        private int ClampIndex(int index, int arrayLength)
        {
            return Mathf.Clamp(index, 0, arrayLength - 1);
        }
    }

}