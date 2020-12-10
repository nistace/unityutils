using UnityEngine;
using UnityEngine.Assertions;

namespace Utils.Types {
// Interpolation between 2 points with a Bezier Curve (cubic spline) 
// Adaptation from the algorithm found at https://www.habrador.com/tutorials/interpolation/2-bezier-curve/
	public class BezierCurve {
		private Vector3   startPosition { get; }
		private Vector3   endPosition   { get; }
		public  Vector3[] controlPoints { get; }

		public  Vector3[] curvePoints    { get; }
		private float[]   segmentLengths { get; }
		public  float     length         { get; }

		public BezierCurve(Vector3 startPosition, Vector3 endPosition, Vector3[] controlPoints, int precision = 100) {
			Assert.IsTrue(controlPoints.Length >= 2);
			Assert.IsTrue(precision >= 1);
			this.startPosition = startPosition;
			this.endPosition = endPosition;
			this.controlPoints = controlPoints;

			length = 0;
			curvePoints = new Vector3[precision + 1];
			segmentLengths = new float[precision];
			for (var i = 0; i <= precision; ++i) {
				curvePoints[i] = DeCasteljausAlgorithm((float) i / precision);
				if (i > 0) {
					segmentLengths[i - 1] = Vector3.Distance(curvePoints[i], curvePoints[i - 1]);
					length += segmentLengths[i - 1];
				}
			}
		}

		public Vector3 GetPosition(Ratio ratio) {
			return GetPosition(ratio * length);
		}

		public Vector3 GetPosition(float distanceTravelled) {
			var midSegmentDistance =
				distanceTravelled; // we will check steps in the bezier curve and stop on the segment on which actual position is. Remaining distance is the distance from the segment start
			var bezierStepIndex = 0;
			while (bezierStepIndex < segmentLengths.Length && segmentLengths[bezierStepIndex] < midSegmentDistance) {
				midSegmentDistance -= segmentLengths[bezierStepIndex];
				bezierStepIndex++;
			}
			var segmentStart = curvePoints[bezierStepIndex];
			var segmentEnd = curvePoints[bezierStepIndex + 1];
			var segmentDirection = segmentEnd - segmentStart;

			return segmentStart + segmentDirection.normalized * midSegmentDistance;
		}

		//The De Casteljau's Algorithm
		private Vector3 DeCasteljausAlgorithm(float t) {
			var layerPositions = new Vector3[controlPoints.Length + 2];
			for (var i = 0; i < controlPoints.Length; ++i) layerPositions[i + 1] = controlPoints[i];
			layerPositions[0] = startPosition;
			layerPositions[layerPositions.Length - 1] = endPosition;
			for (var layer = 1; layer < layerPositions.Length; ++layer)
			for (var i = 0; i < layerPositions.Length - layer; ++i)
				layerPositions[i] = Vector3.Lerp(layerPositions[i], layerPositions[i + 1], t);
			return layerPositions[0];
		}

		internal void OnDrawGizmos() {
			Gizmos.color = Color.white;
			for (var i = 1; i < curvePoints.Length; ++i) {
				Gizmos.DrawLine(curvePoints[i - 1], curvePoints[i]);
			}

			Gizmos.color = Color.green;
			Gizmos.DrawSphere(startPosition, .2f);
			Gizmos.DrawSphere(endPosition, .2f);
			for (var i = 0; i < controlPoints.Length; ++i) {
				Gizmos.DrawSphere(controlPoints[i], .1f);
				if (i > 0) {
					Gizmos.DrawLine(controlPoints[i - 1], controlPoints[i]);
				}
			}
			Gizmos.DrawLine(startPosition, controlPoints[0]);
			Gizmos.DrawLine(endPosition, controlPoints[controlPoints.Length - 1]);
		}
	}
}