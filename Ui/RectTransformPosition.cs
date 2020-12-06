using System;
using UnityEngine;

namespace Utils.Types.Ui {
	[Serializable]
	public class RectTransformPosition {
		[SerializeField] protected Vector2 _anchorMin;
		[SerializeField] protected Vector2 _anchorMax;
		[SerializeField] protected Vector2 _offsetMin;
		[SerializeField] protected Vector2 _offsetMax;

		public Vector2 anchorMin {
			get => _anchorMin;
			set => _anchorMin = value;
		}

		public Vector2 anchorMax {
			get => _anchorMax;
			set => _anchorMax = value;
		}

		public Vector2 offsetMin {
			get => _offsetMin;
			set => _offsetMin = value;
		}

		public Vector2 offsetMax {
			get => _offsetMax;
			set => _offsetMax = value;
		}
	}
}