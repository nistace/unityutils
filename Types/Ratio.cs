using System;
using UnityEngine;
using Utils.Extensions;
using Utils.Random;

namespace Utils.Types {
	[Serializable]
	public struct Ratio : IComparable<Ratio> {
		public static Ratio one  { get; } = 1;
		public static Ratio zero { get; } = 0;

		public static Ratio Random() => new Ratio(UnityEngine.Random.value);

		private static bool Roll(float value, float random, bool winUnder = true) {
			if (value >= 1) return winUnder;
			if (value <= 0) return !winUnder;
			return winUnder == random < value;
		}

		[Range(0, 1)] [SerializeField] private float _value;

		public float value => _value;

		public Ratio(float value) {
			_value = value.Clamp(0, 1);
		}

		public static implicit operator Ratio(float value) => new Ratio(value);
		public static implicit operator float(Ratio r) => r.value;

		public Ratio(float value, float over) {
			_value = value;
			if (value == 0) _value = 0;
			else if (over == 0) _value = 1;
			else _value = value / over;
			_value = this.value.Clamp(0, 1);
		}

		public static Ratio operator +(Ratio r, Ratio r2) => r + r2.value;
		public static Ratio operator -(Ratio r, Ratio r2) => r - r2.value;
		public static Ratio operator *(Ratio r, Ratio r2) => new Ratio(r.value * r2.value);
		public static Ratio operator /(Ratio r, Ratio r2) => new Ratio(r.value, r2.value);
		public static Ratio operator +(Ratio r, float v) => new Ratio(r.value + v);
		public static Ratio operator -(Ratio r, float v) => new Ratio(r.value - v);
		public static bool operator !=(Ratio r, float f) => !(r == f);
		public static bool operator ==(Ratio r, float f) => r.value == f;
		public static float operator *(Ratio r, float v) => r.value * v;
		public static float operator /(Ratio r, float v) => r.value / v;

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

		public override int GetHashCode() => value.GetHashCode();

		public bool Roll(bool winUnder = true) => Roll(value, UnityEngine.Random.value, winUnder);
		public bool NetworkRoll(bool winUnder = true) => Roll(value, SeedRandom.GetNextValue(), winUnder);
		public override string ToString() => $"{value}";
		public string ToStringPc(string format) => $"{(value * 100).ToString(format)}%";
		public int CompareTo(Ratio other) => _value.CompareTo(other._value);
	}
}