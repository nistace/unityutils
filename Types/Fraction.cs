using System;
using UnityEngine;

[Serializable]
public struct Fraction {
	[SerializeField] private int _numerator;
	[SerializeField] private int _denominator;

	public float floatValue => (float) _numerator / _denominator;

	public Fraction(int numerator, int denominator) {
		var positive = numerator > 0 && denominator > 0 || numerator < 0 && denominator < 0;

		var numeratorAbs = Mathf.Abs(numerator);
		var denominatorAbs = Mathf.Abs(denominator);
		for (var simplificationDivisor = 2; simplificationDivisor <= Mathf.Min(numeratorAbs, denominatorAbs); ++simplificationDivisor) {
			while (numeratorAbs % simplificationDivisor == 0 && denominatorAbs % simplificationDivisor == 0) {
				numeratorAbs /= simplificationDivisor;
				denominatorAbs /= simplificationDivisor;
			}
		}

		_numerator = positive ? numeratorAbs : -numeratorAbs;
		_denominator = denominatorAbs;
	}

	public static implicit operator Fraction(int value) => new Fraction(value, 1);
	public static bool operator ==(Fraction first, Fraction second) => first._numerator == second._numerator && first._denominator == second._denominator;
	public static bool operator !=(Fraction first, Fraction second) => !(first == second);
	public static bool operator <(Fraction first, Fraction second) => first <= second && first != second;
	public static bool operator >(Fraction first, Fraction second) => first >= second && first != second;
	public static bool operator <=(Fraction first, Fraction second) => first == second || first.floatValue < second.floatValue;
	public static bool operator >=(Fraction first, Fraction second) => first == second || first.floatValue > second.floatValue;
	public static Fraction operator +(Fraction f, Fraction s) => new Fraction(f._numerator * s._denominator + s._numerator * f._denominator, f._denominator * s._denominator);
	public static Fraction operator -(Fraction fraction) => new Fraction(-fraction._numerator, fraction._denominator);
	public static Fraction operator -(Fraction f, Fraction s) => f + -s;
	public static Fraction operator *(Fraction f, Fraction s) => new Fraction(f._numerator * s._numerator, f._denominator * s._denominator);
	public static Fraction operator /(Fraction f, Fraction s) => new Fraction(f._numerator * s._denominator, f._denominator * s._numerator);

	private bool Equals(Fraction other) => _numerator == other._numerator && _denominator == other._denominator;

	public override bool Equals(object obj) {
		switch (obj) {
			case int i: return Equals(i);
			case Fraction f: return Equals(f);
			default: return false;
		}
	}

	public override int GetHashCode() => floatValue.GetHashCode();

	public string ToString(string format = "n/d") => format.Replace("n", $"{_numerator}").Replace("d", $"{_denominator}");
}