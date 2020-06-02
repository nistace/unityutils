using System.Collections.Generic;
using UnityEngine;

public struct TooltipData {
	public  RectTransform                       target     { get; }
	public  string                              title      => GetParameter(TooltipHolder.titleParameter);
	public  string                              text       => GetParameter(TooltipHolder.textParameter);
	public  string                              shortcut   => GetParameter(TooltipHolder.shortcutParameter);
	private IReadOnlyDictionary<string, string> parameters { get; }

	public bool isNullOrEmpty => string.IsNullOrEmpty(title) && string.IsNullOrEmpty(text) && string.IsNullOrEmpty(shortcut);

	public TooltipData(RectTransform target, IReadOnlyDictionary<string, string> parameters) {
		this.target = target;
		this.parameters = parameters;
	}

	private string GetParameter(string key) => parameters?.Of(key) ?? string.Empty;
}