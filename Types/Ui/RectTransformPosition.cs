using System;
using UnityEngine;

namespace Utils.Types.Ui {
	[Serializable]
	public class RectTransformPosition {
		[SerializeField] protected Vector2    _anchorMin;
		[SerializeField] protected Vector2    _anchorMax;
		[SerializeField] protected RectOffset _offset;

		public Vector2 anchorMin {
			get => _anchorMin;
			set => _anchorMin = value;
		}

		public Vector2 anchorMax {
			get => _anchorMax;
			set => _anchorMax = value;
		}

		public RectOffset offset {
			get => _offset;
			set => _offset = value;
		}
	}
}