using UnityEngine;
using UnityEngine.UI;
using Utils.Events;

namespace Utils.Dropdown {
	public class ImageDropdownOption : MonoBehaviour {
		[SerializeField] protected string _optionKey;
		[SerializeField] protected Button _optionButton;
		[SerializeField] protected Image  _optionImage;

		public StringEvent onClick { get; } = new StringEvent();

		public string optionKey {
			get => _optionKey;
			set => _optionKey = value;
		}

		public Sprite sprite {
			get => _optionImage.sprite;
			set => _optionImage.sprite = value;
		}

		public Color color {
			get => _optionImage.color;
			set => _optionImage.color = value;
		}

		public bool interactable {
			get => _optionButton.interactable;
			set => _optionButton.interactable = value;
		}

		private void OnEnable() => SetListeners(true);
		private void OnDisable() => SetListeners(false);

		private void SetListeners(bool enabled) {
			if (_optionButton) _optionButton.onClick.SetListenerActive(HandleButtonClicked, enabled);
		}

		private void HandleButtonClicked() => onClick.Invoke(_optionKey);
	}
}