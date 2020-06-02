using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour {
	[SerializeField] protected Button           _button;
	[SerializeField] protected bool             _toggledOn;
	[SerializeField] protected ColorBlockHandle _defaultColors  = new ColorBlockHandle();
	[SerializeField] protected ColorBlockHandle _selectedColors = new ColorBlockHandle();

	public Button button {
		get => _button;
		set => _button = value;
	}

	public ColorBlock defaultColors {
		get => _defaultColors;
		set => _defaultColors = value;
	}

	public ColorBlock selectedColors {
		get => _selectedColors;
		set => _selectedColors = value;
	}

	public UnityEvent onClick => _button.onClick;

	public bool toggledOn {
		get => _toggledOn;
		set {
			_toggledOn = value;
			_button.colors = _toggledOn ? _selectedColors : _defaultColors;
		}
	}

	private void Reset() {
		_button = GetComponent<Button>();
		if (!_button) return;
		var colors = _button.colors;
		_defaultColors = colors;
		_selectedColors = colors.With(normalColor: new Color(.5f, .7f, 1, 1), highlightedColor: new Color(.4f, .6f, 1, 1));
	}

#if UNITY_EDITOR
	[ContextMenu("Refresh")] private void Refresh() => toggledOn = toggledOn;
#endif
}