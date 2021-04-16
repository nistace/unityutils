using UnityEngine;
using UnityEngine.UI;

namespace Utils.StaticUtils {
	public static class DiffSet {
		public static void Active(GameObject go, bool active) {
			if (!go) return;
			if (go.activeSelf == active) return;
			go.SetActive(active);
		}

		public static void ActiveGameObject(MonoBehaviour mb, bool active) {
			if (!mb) return;
			Active(mb.gameObject, active);
		}

		public static void Enabled(Behaviour mb, bool enabled) {
			if (!mb) return;
			if (mb.enabled == enabled) return;
			mb.enabled = enabled;
		}

		public static void Color(Graphic gr, Color color) {
			if (!gr) return;
			if (gr.color == color) return;
			gr.color = color;
		}

		public static void Sprite(Image image, Sprite sprite) {
			if (!image) return;
			if (image.sprite == sprite) return;
			image.sprite = sprite;
		}

		public static void Interactable(Selectable selectable, bool interactable) {
			if (!selectable) return;
			if (selectable.interactable == interactable) return;
			selectable.interactable = interactable;
		}

		public static void Bool(Animator animator, int animParam, bool value) {
			if (!animator) return;
			if (animator.GetBool(animParam) == value) return;
			animator.SetBool(animParam, value);
		}
	}
}