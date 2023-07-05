using UnityEngine;

namespace NiUtils.Extensions {
	public static class MeshRendererExtension {
		public static void SetRenderingModeTransparent(this MeshRenderer renderer) => renderer.material.SetRenderingModeTransparent();
		public static void SetRenderingModeOpaque(this MeshRenderer renderer) => renderer.material.SetRenderingModeOpaque();
	}
}