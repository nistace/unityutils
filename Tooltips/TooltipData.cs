using System.Collections.Generic;
using UnityEngine;

public class TooltipData {
	public  RectTransform                       target         { get; }
	public  Vector2                             targetPosition { get; }
	public  bool                                targetIsV2     { get; }
	public  string                              title          => GetParameter(TooltipHolder.titleParameter);
	public  string                              text           => GetParameter(TooltipHolder.textParameter);
	public  string                              shortcut       => GetParameter(TooltipHolder.shortcutParameter);
	private IReadOnlyDictionary<string, string> parameters     { get; }

	public bool isNullOrEmpty => string.IsNullOrEmpty(title) && string.IsNullOrEmpty(text) && string.IsNullOrEmpty(shortcut);

	public TooltipData(RectTransform target, IReadOnlyDictionary<string, string> parameters) {
		this.target = target;
		targetPosition = target.position;
		targetIsV2 = false;
		this.parameters = parameters;
	}

	public TooltipData(Vector2 targetPosition, IReadOnlyDictionary<string, string> parameters) {
		this.targetPosition = targetPosition;
		targetIsV2 = true;
		target = null;
		this.parameters = parameters;
	}

	private string GetParameter(string key) => parameters?.Of(key) ?? string.Empty;
}