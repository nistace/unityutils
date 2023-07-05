using System;
using UnityEngine;

namespace NiUtils.Types {
	[Serializable]
	public struct ColorRange {
		[SerializeField] private Color _first;
		[SerializeField] private Color _second;

		public Color first  => _first;
		public Color second => _second;

		public Color Lerp(float value) {
			if (value <= 0) return _first;
			if (value >= 1) return _second;
			return new Color(Mathf.Lerp(first.r, second.r, value), Mathf.Lerp(first.g, second.g, value), Mathf.Lerp(first.b, second.b, value), Mathf.Lerp(first.a, second.a, value));
		}

		public ColorRange(Color first, Color second) {
			_first = first;
			_second = second;
		}

		public Color Random() {
			return Lerp(UnityEngine.Random.Range(0, 1f));
		}
	}
}