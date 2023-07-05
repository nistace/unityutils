using System;
using UnityEngine;
using UnityEngine.UI;

namespace NiUtils.Extensions {
	public static class ColorExtension {
		public static Color With(this Color c, float? r = null, float? g = null, float? b = null, float? a = null) {
			return new Color(r ?? c.r, g ?? c.g, b ?? c.b, a ?? c.a);
		}

		public static Color With(this Color c, Func<float, float> r = null, Func<float, float> g = null, Func<float, float> b = null, Func<float, float> a = null) {
			return new Color(r?.Invoke(c.r) ?? c.r, g?.Invoke(c.g) ?? c.g, b?.Invoke(c.b) ?? c.b, a?.Invoke(c.a) ?? c.a);
		}

		public static Color Brighter(this Color c, float coefficient = .5f) {
			return new Color(c.r * (1 + coefficient), c.g * (1 + coefficient), c.b * (1 + coefficient), c.a);
		}

		public static Color Darker(this Color c, float coefficient = .5f) {
			return new Color(c.r * (1 - coefficient), c.g * (1 - coefficient), c.b * (1 - coefficient), c.a);
		}

		public static string ToHexaString(this Color c, bool includeOpacity = false) {
			var rgb = $"#{(c.r * 255).Floor():X2}{(c.g * 255).Floor():X2}{(c.b * 255).Floor():X2}";
			return includeOpacity ? $"{rgb}{(c.a * 255).Floor():X2}" : rgb;
		}

		public static ColorBlock With(this ColorBlock c, float? colorMultiplier = null, Color? normalColor = null, Color? disabledColor = null, Color? highlightedColor = null,
			Color? pressedColor = null, Color? selectedColor = null, float? fadeDuration = null) => new ColorBlock {
			colorMultiplier = colorMultiplier ?? c.colorMultiplier,
			normalColor = normalColor ?? c.normalColor,
			disabledColor = disabledColor ?? c.disabledColor,
			highlightedColor = highlightedColor ?? c.highlightedColor,
			pressedColor = pressedColor ?? c.pressedColor,
			selectedColor = selectedColor ?? c.selectedColor,
			fadeDuration = fadeDuration ?? c.fadeDuration
		};
	}
}