using UnityEngine;

namespace Utils.Extensions {
	public static class MaterialExtension {
		private static readonly int dstBlend = Shader.PropertyToID("_DstBlend");
		private static readonly int zWrite   = Shader.PropertyToID("_ZWrite");

		public static void SetRenderingModeTransparent(this Material material) {
			material.SetInt(dstBlend, (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			material.SetInt(zWrite, 0);
			material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = 3000;
		}

		public static void SetRenderingModeOpaque(this Material material) {
			material.SetInt(dstBlend, (int) UnityEngine.Rendering.BlendMode.Zero);
			material.SetInt(zWrite, 1);
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = -1;
		}

		public static void SetEmissionEnabled(this Material material, bool enabled) => material.SetKeywordEnabled("_EMISSION", enabled);

		public static void SetKeywordEnabled(this Material material, string keyword, bool enabled) {
			if (enabled) material.EnableKeyword(keyword);
			else material.DisableKeyword(keyword);
		}
	}
}