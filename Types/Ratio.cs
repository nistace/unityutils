using System;
using UnityEngine;

[Serializable]
public struct Ratio : IComparable<Ratio> {
	public static Ratio one  { get; } = 1;
	public static Ratio zero { get; } = 0;

	public static Ratio Random() {
		return new Ratio(UnityEngine.Random.value);
	}

	private static bool Roll(float value, bool winUnder = true) {
		if (value >= 1) return winUnder;
		if (value <= 0) return !winUnder;
		return winUnder == (UnityEngine.Random.value < value);
	}

	[Range(0, 1)] [SerializeField] private float _value;

	public float value => _value;

	public Ratio(float value) {
		_value = value.Clamp(0, 1);
	}

	public Ratio(float value, float over) {
		_value = value;
		if (value == 0) _value = 0;
		else if (over == 0) _value = 1;
		else _value = value / over;
		_value = this.value.Clamp(0, 1);
	}

	public static Ratio operator +(Ratio r, Ratio r2) {
		return r + r2.value;
	}

	public static Ratio operator -(Ratio r, Ratio r2) {
		return r - r2.value;
	}

	public static Ratio operator *(Ratio r, Ratio r2) {
		return new Ratio(r.value * r2.value);
	}

	public static Ratio operator /(Ratio r, Ratio r2) {
		return new Ratio(r.value, r2.value);
	}

	public static Ratio operator +(Ratio r, float v) {
		return new Ratio(r.value + v);
	}

	public static Ratio operator -(Ratio r, float v) {
		return new Ratio(r.value - v);
	}

	public static bool operator !=(Ratio r, float f) {
		return !(r == f);
	}

	public static bool operator ==(Ratio r, float f) {
		return r.value == f;
	}

	public override bool Equals(object obj) {
		switch (obj) {
			case Ratio ratio:
				return this == ratio;
			case float f:
				return this == f;
			case int i:
				return this == i;
			default:
				return false;
		}
	}

	public override int GetHashCode() {
		return value.GetHashCode();
	}

	public static float operator *(Ratio r, float v) {
		return r.value * v;
	}

	public static float operator /(Ratio r, float v) {
		return r.value / v;
	}

	public static implicit operator Ratio(float value) {
		return new Ratio(value);
	}

	public static implicit operator float(Ratio r) {
		return r.value;
	}

	public bool Roll(bool winUnder = true) {
		return Roll(value, winUnder);
	}

	public override string ToString() {
		return $"{value}";
	}

	public string ToStringPc(string format) {
		return $"{(value * 100).ToString(format)}%";
	}

	public int CompareTo(Ratio other) => _value.CompareTo(other._value);
}