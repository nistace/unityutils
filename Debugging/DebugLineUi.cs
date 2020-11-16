using System;
using TMPro;
using UnityEngine;

public class DebugLineUi : MonoBehaviour {
	[SerializeField] protected TMP_Text _timeText;
	[SerializeField] protected TMP_Text _typeText;
	[SerializeField] protected TMP_Text _infoText;

	public Color color {
		get => _infoText.color;
		set {
			if (_timeText) _timeText.color = value;
			if (_typeText) _typeText.color = value;
			if (_infoText) _infoText.color = value;
		}
	}

	public void Set(string info, string type = null) {
		if (_timeText) _timeText.text = $"[{DateTime.Now:yyMMdd.HH:mm:ss}]";
		if (_typeText) _typeText.text = type ?? string.Empty;
		if (_infoText) _infoText.text = info ?? string.Empty;
	}
}