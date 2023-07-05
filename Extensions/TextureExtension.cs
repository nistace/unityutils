using UnityEngine;

namespace NiUtils.Extensions {
	public static class TextureExtension {
		public static void DrawSpriteOnTop(this Texture2D texture, Sprite sprite, RectInt rect, Color color) {
			if (color.a == 0) return;
			if (!sprite) return;

			for (var x = 0; x < rect.width; ++x)
			for (var y = 0; y < rect.height; ++y) {
				var skillPixel = sprite.texture.GetPixel((int)sprite.textureRect.x + x, (int)sprite.textureRect.y + y) * color;
				if (skillPixel.a == 1) texture.SetPixel(rect.x + x, rect.y + y, skillPixel);
				else if (skillPixel.a > 0) {
					texture.SetPixel(rect.x + x, rect.y + y, Color.Lerp(texture.GetPixel(rect.x + x, rect.y + y), new Color(skillPixel.r, skillPixel.g, skillPixel.b), skillPixel.a));
				}
			}

			texture.Apply();
		}

		public static void FillIn(this Texture2D texture, Color color) {
			for (var x = 0; x < texture.width; ++x)
			for (var y = 0; y < texture.height; ++y) {
				texture.SetPixel(x, y, color);
			}
			texture.Apply();
		}

		public static Texture2D CreateCopy(this Texture2D original) {
			var texture = new Texture2D(original.width, original.height);
			texture.SetPixels(original.GetPixels());
			texture.Apply();
			return texture;
		}
	}
}