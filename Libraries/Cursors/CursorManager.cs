﻿using UnityEngine;

namespace Utils.Libraries {
	public class CursorManager : MonoBehaviour {
		private static CursorManager instance { get; set; }
		private static CursorLibrary library  { get; set; }

		private CursorType cursor         { get; set; }
		private CursorType previousCursor { get; set; }
		private int        frame          { get; set; }
		private float      nextFrameDelay { get; set; }
		private bool       animated       => cursor != null && cursor.frameCount > 1;

		public static void LoadLibrary(CursorLibrary libraryToLoad) {
			library = libraryToLoad;
			if (library) library.Load();
		}

		private void Awake() {
			instance = this;
			Cursor.lockState = Application.isEditor ? CursorLockMode.None : CursorLockMode.Confined;
			SetDefault();
		}

		public static void SetDefault() => SetCursor((CursorType) null);

		public static void SetCursor(string key) => SetCursor(library?[key]);

		public static void SetCursorToPrevious() => SetCursor(instance.previousCursor);

		private static void SetCursor(CursorType cursor) {
			if (cursor == null) cursor = library?.defaultItem;
			if (instance.cursor == cursor) return;
			instance.previousCursor = instance.cursor;
			instance.cursor = cursor;
			instance.frame = 0;
			instance.nextFrameDelay = 0;
			instance.RefreshCursor();
		}

		private void FixedUpdate() {
			if (!animated) return;
			nextFrameDelay += Time.deltaTime;
			if (!(nextFrameDelay > cursor.animationTick)) return;
			nextFrameDelay -= cursor.animationTick;
			frame = (frame + 1) % cursor.frameCount;
			RefreshCursor();
		}

		private void RefreshCursor() {
			Cursor.SetCursor(cursor[frame], cursor.hotspot, CursorMode.Auto);
		}
	}
}