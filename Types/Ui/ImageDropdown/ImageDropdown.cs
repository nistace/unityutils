using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils.Events;

namespace Utils.Dropdown {
	public class ImageDropdown : MonoBehaviour {
		[SerializeField] protected Button              _selectedOptionButton;
		[SerializeField] protected Image               _selectedOptionImage;
		[SerializeField] protected GameObject          _dropdown;
		[SerializeField] protected Transform           _dropdownOptionsContainer;
		[SerializeField] protected ImageDropdownOption _optionPrefab;

		public string value { get; private set; } = string.Empty;

		private Dictionary<string, (Sprite sprite, Color color)> options    { get; } = new Dictionary<string, (Sprite, Color)>();
		private Dictionary<string, ImageDropdownOption>          optionUis  { get; } = new Dictionary<string, ImageDropdownOption>();
		private Queue<ImageDropdownOption>                       optionPool { get; } = new Queue<ImageDropdownOption>();

		public StringEvent onChanged { get; } = new StringEvent();

		private void Awake() {
			HideDropdown();
			_selectedOptionButton.onClick.AddListenerOnce(HandleSelectedOptionButtonClicked);
		}

		public void SetValue(string optionKey) {
			SetValueWithoutNotify(optionKey);
			onChanged.Invoke(value);
		}

		public void SetValueWithoutNotify(string optionKey) {
			value = optionKey;
			if (options.ContainsKey(value)) {
				_selectedOptionImage.sprite = options[value].sprite;
				_selectedOptionImage.color = options[value].color;
			}
			else {
				_selectedOptionImage.sprite = null;
				_selectedOptionImage.color = Color.black;
			}
		}

		public void SetOptions(IEnumerable<KeyValuePair<string, (Sprite, Color)>> newOptions) {
			ClearOptions();
			var keyValuePairs = newOptions as KeyValuePair<string, (Sprite sprite, Color color)>[] ?? newOptions.ToArray();
			foreach (var option in keyValuePairs) {
				var optionUi = GetNewOptionUi();
				optionUi.optionKey = option.Key;
				optionUi.sprite = option.Value.sprite;
				optionUi.color = option.Value.color;
				optionUis.Set(option.Key, optionUi);
				options.Set(option.Key, option.Value);
			}
			SetValue(options.ContainsKey(value) ? value : keyValuePairs.Length > 0 ? keyValuePairs.First().Key : string.Empty);
		}

		private void ClearOptions() {
			options.Clear();
			foreach (var optionUi in optionUis.Values) {
				optionUi.gameObject.SetActive(false);
				optionPool.Enqueue(optionUi);
				optionUi.onClick.RemoveListener(HandleOptionClicked);
			}
			optionUis.Clear();
		}

		private ImageDropdownOption GetNewOptionUi() {
			var newOption = optionPool.Count == 0 ? Instantiate(_optionPrefab, _dropdownOptionsContainer) : optionPool.Dequeue();
			newOption.onClick.AddListenerOnce(HandleOptionClicked);
			newOption.gameObject.SetActive(true);
			return newOption;
		}

		private void HandleOptionClicked(string optionKey) {
			HideDropdown();
			SetValue(optionKey);
		}

		private void HandleSelectedOptionButtonClicked() {
			if (_dropdown.activeSelf) HideDropdown();
			else ShowDropdown();
		}

		public void SetInteractable(bool interactable) {
			_selectedOptionButton.interactable = interactable;
			if (!interactable) HideDropdown();
		}

		private void ShowDropdown() {
			_dropdown.SetActive(true);
			_dropdown.transform.SetParent(transform.root);
		}

		private void HideDropdown() {
			_dropdown.SetActive(false);
			_dropdown.transform.SetParent(transform);
		}
	}
}