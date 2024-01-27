using UnityEngine;

namespace NiUtils.StaticUtils {
	public static class TouchScreenKeyboards {
		public static TouchScreenKeyboard CurrentKeyboard { get; set; }
		private static AndroidJavaObject UnityPlayerView { get; set; }
		private static AndroidJavaObject UnityPlayerRect { get; set; }

		private static int GetKeyboardHeight() {
#if UNITY_ANDROID && !UNITY_EDITOR
			UnityPlayerView ??= new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer")
				.Call<AndroidJavaObject>("getView");
			UnityPlayerRect ??= new AndroidJavaObject("android.graphics.Rect");

			UnityPlayerView.Call("getWindowVisibleDisplayFrame", UnityPlayerRect);

			return Screen.height - UnityPlayerRect.Call<int>("height");
#endif
			return 0;
		}

		public static float GetKeyboardHeightRatio() => (float)GetKeyboardHeight() / Screen.height;
	}
}