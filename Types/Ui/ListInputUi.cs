using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Events;

namespace Utils.Types.Ui {
	public class ListInputUi : MonoBehaviour {
		[SerializeField] protected bool     _interactable = true;
		[SerializeField] protected L10NText _currentItemTextKey;
		[SerializeField] protected TMP_Text _currentItemTextValue;
		[SerializeField] protected bool     _optionsAreKeys = true;
		[SerializeField] protected Button   _previousItemButton;
		[SerializeField] protected Button   _nextItemButton;
		[SerializeField] protected bool     _loop = true;

		public  bool         interactable => _interactable;
		private List<string> options      { get; } = new List<string>();
		private int          currentIndex { get; set; }

		public IntEvent onValueChanged { get; } = new IntEvent();

		private void Awake() {
			Refresh();
			_nextItemButton.onClick.AddListenerOnce(IncrementIndex);
			_previousItemButton.onClick.AddListenerOnce(DecrementIndex);
		}

		public void Set(IEnumerable<string> optionsAsKeys, int currentIndex, bool withoutNotify = true) {
			options.Clear();
			options.AddRange(optionsAsKeys);
			if (withoutNotify) SetValueWithoutNotify(currentIndex);
			else SetValue(currentIndex);
			Refresh();
		}

		private void Refresh() {
			var hasItems = options.Count > 0;
			_currentItemTextKey.enabled = _optionsAreKeys;
			if (_optionsAreKeys) _currentItemTextKey.key = hasItems ? options[currentIndex] : string.Empty;
			else _currentItemTextValue.text = hasItems ? options[currentIndex] : string.Empty;
			_previousItemButton.interactable = interactable && hasItems && (_loop || currentIndex > 0);
			_nextItemButton.interactable = interactable && hasItems && (_loop || currentIndex < options.Count - 1);
		}

		public void SetValueWithoutNotify(int index) {
			currentIndex = index.Clamp(0, options.Count - 1);
			Refresh();
		}

		public void SetInteractable(bool interactable) {
			_interactable = interactable;
			Refresh();
		}

		private void IncrementIndex() => SetValue(_loop ? (currentIndex + 1).PosMod(options.Count) : (currentIndex + 1).Clamp(0, options.Count - 1));
		private void DecrementIndex() => SetValue(_loop ? (currentIndex - 1).PosMod(options.Count) : (currentIndex - 1).Clamp(0, options.Count - 1));

		private void SetValue(int index) {
			SetValueWithoutNotify(index);
			onValueChanged.Invoke(currentIndex);
		}
	}
}