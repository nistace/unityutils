using UnityEngine;

namespace Utils.Extensions {
	public static class AnimationExtension {
		public static void Restart(this Animation animation) {
			animation.Stop();
			animation.Play();
		}
	}
}