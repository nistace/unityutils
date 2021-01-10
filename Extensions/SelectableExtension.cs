using UnityEngine.UI;

namespace Utils.Extensions {
	public static class SelectableExtension {
		public static void SetInteractableIfDiff(this Selectable selectable, bool newValue) {
			if (!selectable) return;
			if (selectable.interactable == newValue) return;
			selectable.interactable = newValue;
		}
	}
}