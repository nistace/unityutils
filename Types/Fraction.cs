using UnityEngine;

public struct Fraction {
	public int numerator   { get; }
	public int denominator { get; }

	public float floatValue => (float) numerator / denominator;

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

		this.numerator = positive ? numeratorAbs : -numeratorAbs;
		this.denominator = denominatorAbs;
	}

	public static implicit operator Fraction(int value) => new Fraction(value, 1);
	public static bool operator ==(Fraction first, Fraction second) => first.numerator == second.numerator && first.denominator == second.denominator;
	public static bool operator !=(Fraction first, Fraction second) => !(first == second);
	public static bool operator <(Fraction first, Fraction second) => first <= second && first != second;
	public static bool operator >(Fraction first, Fraction second) => first >= second && first != second;
	public static bool operator <=(Fraction first, Fraction second) => first == second || first.floatValue < second.floatValue;
	public static bool operator >=(Fraction first, Fraction second) => first == second || first.floatValue > second.floatValue;
	public static Fraction operator +(Fraction f, Fraction s) => new Fraction(f.numerator * s.denominator + s.numerator * f.denominator, f.denominator * s.denominator);
	public static Fraction operator -(Fraction fraction) => new Fraction(-fraction.numerator, fraction.denominator);
	public static Fraction operator -(Fraction f, Fraction s) => f + -s;
	public static Fraction operator *(Fraction f, Fraction s) => new Fraction(f.numerator * s.numerator, f.denominator * s.denominator);
	public static Fraction operator /(Fraction f, Fraction s) => new Fraction(f.numerator * s.denominator, f.denominator * s.numerator);

	private bool Equals(Fraction other) => numerator == other.numerator && denominator == other.denominator;

	public override bool Equals(object obj) {
		switch (obj) {
			case int i: return Equals(i);
			case Fraction f: return Equals(f);
			default: return false;
		}
	}

	public override int GetHashCode() => floatValue.GetHashCode();

	public string ToString(string format = "n/d") => format.Replace("n", $"{numerator}").Replace("d", $"{denominator}");
}