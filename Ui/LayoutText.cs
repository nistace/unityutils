using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
[RequireComponent(typeof(TMPro.TMP_Text))]
public class LayoutText : MonoBehaviour {
	[SerializeField] protected TMPro.TMP_Text _text;
	[SerializeField] protected LayoutElement  _layout;
	[SerializeField] protected FloatRange     _ratioBounds = new FloatRange(2, 5);
	[SerializeField] protected float          _minWidth    = 200;
	[SerializeField] protected float          _maxWidth    = 600;

	public string text {
		get => _text?.text;
		set {
			if (!_text) return;
			_text.text = value;
			if (!_layout) return;
			var preferredValues = _text.GetPreferredValues(value);
			var preferredRatio = preferredValues.x / preferredValues.y;
			var boundedRatio = preferredRatio.Clamp(_ratioBounds);
			_layout.preferredWidth = (preferredValues.y * boundedRatio).Clamp(_minWidth, _maxWidth);
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
	[ContextMenu("Refresh")]
	public void Refresh() {
		text = text;
	}
#endif
}