using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TooltipHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public static string titleParameter    { get; } = "__TITLE";
	public static string textParameter     { get; } = "__TEXT";
	public static string shortcutParameter { get; } = "__SHORTCUT";

	private TooltipData? shownData { get; set; }

	protected          RectTransform rectTransform { get; private set; }
	protected abstract TooltipUi     uiModel       { get; }

	public void OnPointerEnter(PointerEventData eventData) => Show();
	public void OnPointerExit(PointerEventData eventData) => Hide();

	protected virtual void Awake() {
		rectTransform = GetComponent<RectTransform>();
	}

	private void Show() {
		shownData = GetShowData();
		if (shownData == null) return;
		TooltipOverlayUi.Show(uiModel, shownData.Value);
	}

	protected abstract TooltipData? GetShowData();

	private void Hide() {
		if (shownData == null) return;
		TooltipOverlayUi.Hide(shownData.Value);
		shownData = null;
	}

	private void OnDisable() => Hide();
}