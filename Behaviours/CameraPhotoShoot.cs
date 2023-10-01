using System.IO;
using UnityEngine;

namespace NiUtils {
	public class CameraPhotoShoot : MonoBehaviour {
		[SerializeField] protected string output = "ui.png";

#if UNITY_EDITOR
		[ContextMenu("Shoot")]
		private void Shoot() {
			var Cam = GetComponentInChildren<Camera>();

			var currentRT = RenderTexture.active;
			RenderTexture.active = Cam.targetTexture;

			Cam.Render();

			var Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height);
			Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
			Image.Apply();
			RenderTexture.active = currentRT;

			var Bytes = Image.EncodeToPNG();

			var path = $"{Application.dataPath}/Textures/{output}.png";
			File.WriteAllBytes(path, Bytes);
		}
#endif
	}
}