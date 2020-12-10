using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Events;
using Utils.Extensions;
using Utils.Libraries;

public class CheckboxUi : MonoBehaviour {
	[SerializeField] protected Button   _button;
	[SerializeField] protected Image    _boxImage;
	[SerializeField] protected TMP_Text _text;

	public bool isChecked {
		get => _boxImage.sprite == Sprites.Of("checkbox.checked");
		set => _boxImage.sprite = Sprites.Of($"checkbox.{(value ? "checked" : "empty")}");
	}

	public string text {
		get => _text.text;
		set => _text.text = value;
	}

	public BoolEvent onChanged { get; } = new BoolEvent();

	private void Awake() {
		_button.onClick.AddListenerOnce(Toggle);
	}

	private void Toggle() {
		isChecked = !isChecked;
		onChanged.Invoke(isChecked);
	}
}