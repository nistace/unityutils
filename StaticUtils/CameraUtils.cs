using UnityEngine;

namespace NiUtils.StaticUtils {
	public static class CameraUtils {
		private static Camera cameraInstance { get; set; }
		public static  Camera main           => cameraInstance ? cameraInstance : cameraInstance = Camera.main;
	}
}