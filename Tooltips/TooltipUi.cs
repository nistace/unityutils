using UnityEngine;
using UnityEngine.UI;

public class TooltipUi : MonoBehaviour {
	[Header("Components")] [SerializeField] protected GameObject     _header;
	[SerializeField]                        protected GameObject     _separationLine;
	[SerializeField]                        protected GameObject     _content;
	[Header("Texts")] [SerializeField]      protected TMPro.TMP_Text _title;
	[SerializeField]                        protected LayoutText     _text;
	[SerializeField]                        protected TMPro.TMP_Text _shortcut;
	[SerializeField]                        protected Vector2        _positionDelta;

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
		var rect = target.rect;
		Relocate(target.position, rect.min, rect.max);
	}

	private void Relocate(Vector2 target) => Relocate(target, Vector2.zero, Vector2.zero);

	private void Relocate(Vector2 position, Vector2 targetMin, Vector2 targetMax) {
		var left = position.x < containerRect.width / 2;
		var bottom = position.y < containerRect.height / 2;

		rectTransform.pivot = new Vector2(position.x / containerRect.width, bottom ? 0 : 1);
		rectTransform.position = position + new Vector2(left ? targetMax.x + _positionDelta.x : targetMin.x - _positionDelta.x, bottom ? targetMax.y + _positionDelta.y : targetMin.y - _positionDelta.y);
	}

	public void RefreshGraphicsOpacity(Ratio opacity) => allGraphics.ForEach(t => t.color = t.color.With(a: opacity));

	public virtual void Set(TooltipData data) {
		if (_header) _header.gameObject.SetActive(!string.IsNullOrEmpty(data.title) || !string.IsNullOrEmpty(data.shortcut));
		if (_content) _content.gameObject.SetActive(!string.IsNullOrEmpty(data.text));
		if (_separationLine) _separationLine.gameObject.SetActive(_header.activeSelf && _content.activeSelf);
		if (_title) _title.text = data.title;
		if (_text) _text.text = data.text;
		if (_shortcut) _shortcut.text = data.shortcut;
		if (data.targetIsV2) Relocate(data.targetPosition);
		else Relocate(data.target);
	}
}