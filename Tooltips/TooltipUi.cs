using UnityEngine;
using UnityEngine.UI;

public class TooltipUi : MonoBehaviour {
	[Header("Components")] [SerializeField] protected GameObject     _header;
	[SerializeField]                        protected GameObject     _separationLine;
	[SerializeField]                        protected GameObject     _content;
	[Header("Texts")] [SerializeField]      protected TMPro.TMP_Text _title;
	[SerializeField]                        protected LayoutText     _text;
	[SerializeField]                        protected TMPro.TMP_Text _shortcut;

	private Graphic[]     allGraphics   { get; set; }
	private RectTransform rectTransform { get; set; }
	private Rect          containerRect { get; set; }

	private void Awake() {
		rectTransform = GetComponent<RectTransform>();
		allGraphics = GetComponentsInChildren<Graphic>();
		var container = transform.parent.GetComponent<RectTransform>();
		var rect = container.rect;
		var position = container.position;
		containerRect = new Rect(rect.min.x + position.x, rect.min.y + position.y, rect.width, rect.height);
		_text.maxWidth = (containerRect.width / 2.1f);
	}

	private void Relocate(RectTransform target) {
		Vector2 targetPosition = target.position;
		var targetRect = target.rect;

		var left = targetPosition.x < containerRect.width / 2;
		var bottom = targetPosition.y < containerRect.height / 2;

		rectTransform.pivot = new Vector2(targetPosition.x / containerRect.width, bottom ? 0 : 1);
		rectTransform.position = targetPosition + new Vector2(left ? targetRect.xMax : targetRect.xMin, bottom ? targetRect.yMax : targetRect.yMin);
	}

	public void RefreshGraphicsOpacity(Ratio opacity) => allGraphics.ForEach(t => t.color = t.color.With(a: opacity));

	public virtual void Set(TooltipData data) {
		if (_header) _header.gameObject.SetActive(!string.IsNullOrEmpty(data.title) || !string.IsNullOrEmpty(data.shortcut));
		if (_content) _content.gameObject.SetActive(!string.IsNullOrEmpty(data.text));
		if (_separationLine) _separationLine.gameObject.SetActive(_header.activeSelf && _content.activeSelf);
		if (_title) _title.text = data.title;
		if (_text) _text.text = data.text;
		if (_shortcut) _shortcut.text = data.shortcut;
		Relocate(data.target);
	}
}