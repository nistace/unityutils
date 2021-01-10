﻿using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Events;
using Utils.Extensions;
using Utils.Libraries;

namespace Utils.Ui {
	public class ListInputUi : Selectable {
		[SerializeField] protected L10NText _currentItemTextKey;
		[SerializeField] protected TMP_Text _currentItemTextValue;
		[SerializeField] protected bool     _optionsAreKeys = true;
		[SerializeField] protected Button   _previousItemButton;
		[SerializeField] protected Button   _nextItemButton;
		[SerializeField] protected bool     _loop    = true;
		[SerializeField] protected string[] _options = { };
		[SerializeField] protected int      _value;

		public new bool interactable {
			get => base.interactable;
			set {
				base.interactable = value;
				Refresh();
			}
		}

		public int value {
			get => _value;
			set {
				SetValueWithoutNotify(value);
				onValueChanged.Invoke(_value);
			}
		}

		public IntEvent onValueChanged { get; } = new IntEvent();

		protected override void Awake() {
			base.Awake();
			Refresh();
			_nextItemButton.onClick.AddListenerOnce(IncrementIndex);
			_previousItemButton.onClick.AddListenerOnce(DecrementIndex);
		}

		public void Set(IEnumerable<string> options, int currentIndex, bool withoutNotify = true) {
			_options = options.ToArray();
			if (withoutNotify) SetValueWithoutNotify(currentIndex);
			else value = currentIndex;
			Refresh();
		}

		private void Refresh() {
			if (_currentItemTextKey && _currentItemTextKey.enabled != _optionsAreKeys) _currentItemTextKey.enabled = _optionsAreKeys;
			var valueOption = _options.GetSafe(_value, string.Empty);
			if (_currentItemTextKey && _optionsAreKeys && _currentItemTextKey.key != valueOption) _currentItemTextKey.key = valueOption;
			if (_currentItemTextValue && !_optionsAreKeys && _currentItemTextValue.text != valueOption) _currentItemTextValue.text = valueOption;
			_previousItemButton?.SetInteractableIfDiff(interactable && _options.Length > 1 && (_loop || _value > 0));
			_nextItemButton?.SetInteractableIfDiff(interactable && _options.Length > 1 && (_loop || _value < _options.Length - 1));
		}

		public void SetValueWithoutNotify(int value) {
			_value = value;
			if (_loop) _value = _value.PosMod(_options.Length);
			_value = _value.Clamp(0, _options.Length - 1);
			Refresh();
		}

		private void IncrementIndex() => value++;
		private void DecrementIndex() => value--;

#if UNITY_EDITOR
		protected override void OnValidate() {
			base.OnValidate();
			SetValueWithoutNotify(_value);
		}
#endif
	}
}