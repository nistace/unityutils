using System;
using NiUtils.Extensions;
using UnityEngine;

namespace NiUtils.Types {
	[Serializable]
	public struct FloatRange {
		/// <summary> 0 - 0 </summary>
		public static readonly FloatRange zero = new FloatRange { _min = 0, _max = 0 };

		/// <summary> 1 - 1</summary>
		public static readonly FloatRange one = new FloatRange { _min = 1, _max = 1 };

		/// <summary> 0 - 1 </summary>
		public static readonly FloatRange unit = new FloatRange { _min = 0, _max = 1 };

		[SerializeField] private float _min;
		[SerializeField] private float _max;

		public float min   => _min;
		public float max   => _max;
		public float delta => max - min;

		public FloatRange(float min, float max) {
			_min = min;
			_max = max;
		}

		public static implicit operator FloatRange((int min, int max) values) => new FloatRange(values.min, values.max);
		public static implicit operator FloatRange((int min, float max) values) => new FloatRange(values.min, values.max);
		public static implicit operator FloatRange((float min, float max) values) => new FloatRange(values.min, values.max);
		public static implicit operator FloatRange((float min, int max) values) => new FloatRange(values.min, values.max);

		public float Random() => UnityEngine.Random.Range(_min, _max);
		public float ValueAt(Ratio ratio) => _min + (_max - _min) * ratio.value;
		public Ratio RatioOf(float value) => (value - _min) / (_max - _min);
		public static FloatRange operator +(FloatRange fr1, FloatRange fr2) => new FloatRange(fr1._min + fr2._min, fr1._max + fr2._max);
		public static FloatRange operator -(FloatRange fr1, FloatRange fr2) => new FloatRange(fr1._min - fr2._min, fr1._max - fr2._max);
		public static FloatRange operator *(FloatRange fr, float value) => new FloatRange(fr._min * value, fr._max * value);
		public static FloatRange operator /(FloatRange fr, float value) => new FloatRange(fr._min / value, fr._max / value);
		public static implicit operator FloatRange(IntRange range) => new FloatRange(range.min, range.max);
		public IntRange Round() => new IntRange(min.Round(), max.Round());
		public FloatRange Pow(int pow) => new FloatRange(Mathf.Pow(min, pow), Mathf.Pow(max, pow));
		public float Clamp(float value) => Mathf.Clamp(value, min, max);
	}
}