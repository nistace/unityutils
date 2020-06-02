using System;
using UnityEngine;

[Serializable]
public struct RatioRange {
	public static readonly RatioRange zero = new RatioRange {_min = 0, _max = 0};
	public static readonly RatioRange one  = new RatioRange {_min = 1, _max = 1};
	public static readonly RatioRange unit = new RatioRange {_min = 0, _max = 1};

	[SerializeField] private Ratio _min;
	[SerializeField] private Ratio _max;

	public Ratio min   => _min;
	public Ratio max   => _max;
	public Ratio delta => _max - _min;

	public RatioRange(float min, float max) {
		_min = min;
		_max = max;
	}

	public float Random() {
		return UnityEngine.Random.Range(_min, _max);
	}

	public float ValueAt(Ratio ratio) {
		return _min + (_max - _min) * ratio.value;
	}

	public static implicit operator RatioRange(IntRange range) {
		return new RatioRange(range.min, range.max);
	}

	public static implicit operator RatioRange(FloatRange range) {
		return new RatioRange(range.min, range.max);
	}
}