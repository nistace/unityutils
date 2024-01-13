using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;

namespace NiUtils.StaticUtils {
	public static class DeviceUtils {
		public static string GetPhysicalAddress() {
#if !UNITY_EDITOR && UNITY_ANDROID
			// Get Android ID
			var clsUnity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			var objActivity = clsUnity.GetStatic<AndroidJavaObject>("currentActivity");
			var objResolver = objActivity.Call<AndroidJavaObject>("getContentResolver");
			var clsSecure = new AndroidJavaClass("android.provider.Settings$Secure");
			var androidId = clsSecure.CallStatic<string>("getString", objResolver, "android_id");

			// Encrypt bytes with md5
			var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			var hashBytes = md5.ComputeHash(new System.Text.UTF8Encoding().GetBytes(androidId));
			var hashString = hashBytes.Aggregate("", (current, hashByte) => current + System.Convert.ToString(hashByte, 16).PadLeft(2, '0'));
			return hashString.PadLeft(32, '0');
#else
			var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			foreach (var adapter in networkInterfaces) {
				var address = adapter.GetPhysicalAddress();
				if (!string.IsNullOrEmpty(address.ToString())) {
					return address.ToString();
				}
			}
			return string.Empty;
#endif
		}
	}
}