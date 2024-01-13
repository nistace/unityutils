using UnityEngine;

namespace NiUtils.StaticUtils {
	public static class TouchScreenKeyboards {
		public static TouchScreenKeyboard CurrentKeyboard { get; set; }

		public static int GetKeyboardHeight() {
#if UNITY_ANDROID && !UNITY_EDITOR
			using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				var View = unityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");

				using (var Rct = new AndroidJavaObject("android.graphics.Rect")) {
					View.Call("getWindowVisibleDisplayFrame", Rct);

					return Screen.height - Rct.Call<int>("height");
				}
			}
#endif

			return 0;
		}

		public static float GetKeyboardHeightRatio() => (float)GetKeyboardHeight() / Screen.height;
	}
}