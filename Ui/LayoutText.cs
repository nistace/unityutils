using NiUtils.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace NiUtils.Ui {
	[RequireComponent(typeof(LayoutElement))]
	[RequireComponent(typeof(TMPro.TMP_Text))]
	public class LayoutText : MonoBehaviour {
		[SerializeField] protected TMPro.TMP_Text _text;
		[SerializeField] protected LayoutElement  _layout;
		[SerializeField] protected float          _minWidth = 200;
		[SerializeField] protected float          _maxWidth = 600;

		public string text {
			get => _text?.text;
			set {
				if (!_text) return;
				_text.text = value;
				if (!_layout) return;
				var preferredValues = _text.GetPreferredValues(value);
				var newPreferredWidth = preferredValues.x.Clamp(_minWidth, _maxWidth);
				if (_layout.preferredWidth != newPreferredWidth) _layout.preferredWidth = newPreferredWidth;
			}
		}

		public float minWidth {
			get => _minWidth;
			set => _minWidth = value;
		}

		public float maxWidth {
			get => _maxWidth;
			set => _maxWidth = value;
		}

		private void Reset() {
			_text = GetComponent<TMPro.TMP_Text>();
			_layout = GetComponent<LayoutElement>();
		}

#if UNITY_EDITOR
		private void OnValidate() => text = text;
#endif
	}
}