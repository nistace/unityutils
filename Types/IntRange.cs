using System;
using UnityEngine;

[Serializable]
public struct IntRange {
	public override bool Equals(object obj) {
		return obj is IntRange other && Equals(other);
	}

	public override int GetHashCode() => (min * 397) ^ max;

	public static readonly IntRange zero = new IntRange {_min = 0, _max = 0};
	public static readonly IntRange one  = new IntRange {_min = 1, _max = 1};
	public static readonly IntRange unit = new IntRange {_min = 0, _max = 1};

	[SerializeField] private int _min;
	[SerializeField] private int _max;

	public int min => _min;
	public int max => _max;

	public IntRange(int min, int max) {
		_min = min;
		_max = max;
	}

	public int Random() => UnityEngine.Random.Range(_min, _max + 1);

	public float FloatRandom() => UnityEngine.Random.Range((float) _min, _max);

	public static IntRange operator *(IntRange orig, int amount) {
		return new IntRange(orig._min * amount, orig._max * amount);
	}

	public static FloatRange operator *(IntRange orig, float amount) {
		return new FloatRange(orig._min * amount, orig._max * amount);
	}

	public static IntRange operator +(IntRange ir1, IntRange ir2) {
		return new IntRange(ir1._min + ir2._min, ir1._max + ir2._max);
	}

	public static IntRange operator -(IntRange ir1, IntRange ir2) {
		return new IntRange(ir1._min - ir2._min, ir1._max - ir2._max);
	}

	public static bool operator ==(IntRange ir1, IntRange ir2) => ir1.Equals(ir2);
	public static bool operator !=(IntRange ir1, IntRange ir2) => !(ir1 == ir2);

	private bool Equals(IntRange other) {
		return min == other.min && max == other.max;
	}

	/// <summary>if both values are equal, returns the value, otherwise returns a "m - M" formatted string</summary>
	public string ToRangeString() {
		if (min == max) return $"{min}";
		return $"{min} - {max}";
	}
}