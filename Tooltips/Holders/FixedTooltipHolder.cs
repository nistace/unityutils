using System.Collections.Generic;
using UnityEngine;

public class FixedTooltipHolder : TooltipHolder {
	[SerializeField] protected string                     _title;
	[SerializeField] protected string                     _text;
	[SerializeField] protected bool                       _keys = true;
	protected override         TooltipUi                  uiModel    => null;
	private                    Dictionary<string, string> parameters { get; } = new Dictionary<string, string>();

	public void SetAsKeys(string title, string text) => Set(title, text, true);
	public void SetAsValues(string title, string text) => Set(title, text, false);

	private void Set(string title, string text, bool keys) {
		_title = title;
		_text = text;
		_keys = keys;
	}

	protected override TooltipData GetShowData() {
		parameters.Set(titleParameter, GetValue(_title));
		parameters.Set(textParameter, GetValue(_text));
		return new TooltipData(rectTransform, parameters);
	}

	private string GetValue(string component) {
		if (string.IsNullOrEmpty(component)) return null;
		if (_keys) return Localisation.Map(component);
		return component;
	}
}