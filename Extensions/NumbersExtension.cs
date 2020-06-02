using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class NumbersExtension {
	public static int Ceiling(this float f) {
		if (f == (int) f) return (int) f;
		if (f < 0) return (int) f;
		return (int) f + 1;
	}

	public static int Floor(this float f) {
		if ((int) f == f) return (int) f;
		if (f < 0) return (int) f - 1;
		return (int) f;
	}

	public static int Round(this float f) {
		var ceiling = f.Ceiling();
		if (ceiling - f < .5f) return ceiling;
		return ceiling - 1;
	}

	public static int RandomRound(this float f) {
		var floor = f.Floor();
		if (floor == f) return floor;
		if ((f - floor).Ratio().Roll()) return floor + 1;
		return floor;
	}

	public static int RoundAwayFrom(this float f, float pivot) {
		if ((int) f == f) return (int) f;
		if (f < pivot) return Floor(f);
		return Ceiling(f);
	}

	public static int PosMod(this int i, int mod) {
		if (mod <= 0) return 0;
		var iMod = i % mod;
		if (iMod < 0) return iMod + mod;
		return iMod;
	}

	public static float PosMod(this float i, int mod) {
		if (mod <= 0) return 0;
		var iMod = i % mod;
		if (iMod < 0) return iMod + mod;
		return iMod;
	}

	public static float FloatFloor(this int number, float unit) {
		return ((float) number).FloatFloor(unit);
	}

	public static float FloatFloor(this float number, float unit) {
		if (unit < 0) return float.NaN;
		if (unit != 0) return (number / unit).Floor() * unit;
		if (number > 0) return float.PositiveInfinity;
		if (number < 0) return float.NegativeInfinity;
		return float.NaN;
	}

	public static float FloatRound(this float number, float unit) {
		var floor = number.FloatFloor(unit);
		if (Mathf.Abs(number - floor) < Mathf.Abs(number - (floor + unit))) return floor;
		return floor + unit;
	}

	public static bool Between(this float value, float min, float max, bool includeBounds = true) => value.Between(min, max, includeBounds, includeBounds);

	public static bool Between(this float value, FloatRange range, bool includeBounds = true) => value.Between(range.min, range.max, includeBounds, includeBounds);

	private static bool Between(this float value, float min, float max, bool minIncluded, bool maxIncluded) => value > min && value < max || minIncluded && value == min || maxIncluded && value == max;

	public static int RoundUpToMultipleOf(this int value, int divisor) {
		if (value.PosMod(divisor) == 0) return value;
		return value + divisor - value.PosMod(divisor);
	}

	public static int RoundUpToPowerOf(this float value, int divisor) {
		var powDivisor = 1;
		while (powDivisor < value) powDivisor *= divisor;
		return powDivisor;
	}

	public static string ToDiffString(this float f, string format) {
		return (f >= 0 ? "+" : "-") + Mathf.Abs(f).ToString(format);
	}

	public static int Sum1ToN(this int x) {
		return (x * (x + 1)) / 2;
	}

	public static Ratio Ratio(this float number, float over = 1) => new Ratio(number / over);
	public static Ratio Ratio(this int number, float over = 1) => ((float) number).Ratio(over);
	public static Ratio RatioPc(this int number) => ((float) number).RatioPc();
	public static Ratio RatioPc(this float number) => number.Ratio(100);

	public static float Average(this IEnumerable<float> numbers, float defaultValue = float.PositiveInfinity) {
		var enumerable = numbers as float[] ?? numbers.ToArray();
		var cnt = enumerable.Length;
		if (cnt == 0) return defaultValue;
		return enumerable.Sum() / cnt;
	}

	public static float DecimalPart(this float f) {
		return Mathf.Abs((f - (int) f));
	}

	public static int Clamp(this int i, int min, int max) {
		return Mathf.Clamp(i, min, max);
	}

	public static float Clamp(this float f, float min, float max) => Mathf.Clamp(f, min, max);
	public static float Clamp(this float f, FloatRange range) => f.Clamp(range.min, range.max);

	public static Ratio Clamp(this Ratio r, Ratio min, Ratio max) {
		return r.value.Clamp(min, max);
	}

	public static float AtLeast(this float f, float atLeast) {
		return Mathf.Max(f, atLeast);
	}

	public static float MoveTowards(this float f, float target, float step) => f < target ? Mathf.Min(f + step, target) : Mathf.Max(f - step, target);
}