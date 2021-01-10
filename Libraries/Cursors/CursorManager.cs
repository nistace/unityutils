using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Coroutines;
using Utils.Extensions;

namespace Utils.Libraries {
	public class CursorManager : MonoBehaviour {
		private static CursorManager instance { get; set; }
		private static CursorLibrary library  { get; set; }

		private CursorType                                       cursor             { get; set; }
		private CursorType                                       previousCursor     { get; set; }
		private SingleCoroutine                                  animationCoroutine { get; set; }
		private Dictionary<CursorType, IReadOnlyList<Texture2D>> builtCursorFrames  { get; } = new Dictionary<CursorType, IReadOnlyList<Texture2D>>();

		public static void LoadLibrary(CursorLibrary libraryToLoad) {
			library = libraryToLoad;
			if (!library) return;
			library.Load();
			instance.BuildFrames();
			SetDefault();
		}

		private void BuildFrames() {
			builtCursorFrames.Clear();
			builtCursorFrames.SetAll(library.allItems.Select(t => new KeyValuePair<CursorType, IReadOnlyList<Texture2D>>(t, BuildCursorTextureArray(t))));
			if (library.defaultItem && !builtCursorFrames.ContainsKey(library.defaultItem)) builtCursorFrames.Add(library.defaultItem, BuildCursorTextureArray(library.defaultItem));
		}

		private static IReadOnlyList<Texture2D> BuildCursorTextureArray(CursorType cursor) => cursor.frames.Select(t => GetColoredTexture(t, cursor.color)).ToArray();

		private static Texture2D GetColoredTexture(Texture2D baseTexture, Color color) {
			if (color == Color.white) return baseTexture;
			var newTexture = new Texture2D(baseTexture.width, baseTexture.height, TextureFormat.RGBA32, false);
			for (var x = 0; x < newTexture.width; ++x)
			for (var y = 0; y < newTexture.height; ++y)
				newTexture.SetPixel(x, y, baseTexture.GetPixel(x, y) * color);
			return newTexture;
		}

		private void Awake() {
			if (instance) return;
			instance = this;
			Cursor.lockState = Application.isEditor ? CursorLockMode.None : CursorLockMode.Confined;
			animationCoroutine = new SingleCoroutine(this);
			SetDefault();
		}

		public static void SetDefault() => SetCursor((CursorType) null);

		public static void SetCursor(string key) => SetCursor(library?[key]);

		public static void SetCursorToPrevious() => SetCursor(instance.previousCursor);

		private static void SetCursor(CursorType cursor) {
			var setCursor = cursor ? cursor : library?.defaultItem;
			if (instance.cursor == setCursor) return;
			instance.previousCursor = instance.cursor;
			instance.cursor = setCursor;
			instance.RefreshCursor(0);
			instance.animationCoroutine.Start(instance.DoAnimateCurrentCursor());
		}

		private IEnumerator DoAnimateCurrentCursor() {
			var frame = 0;
			var nextFrameProgress = 0f;
			while (cursor && cursor.animationTick > 0 && cursor.frameCount > 1) {
				while (nextFrameProgress < 1) {
					nextFrameProgress += Time.deltaTime / cursor.animationTick;
					yield return null;
				}
				frame = (frame + 1) % cursor.frameCount;
				RefreshCursor(frame);
				nextFrameProgress = 0;
			}
		}

		private void RefreshCursor(int frame) {
			if (!cursor) return;
			Cursor.SetCursor(builtCursorFrames.Of(cursor)?.GetSafe(frame), cursor.hotspot, CursorMode.Auto);
		}
	}
}