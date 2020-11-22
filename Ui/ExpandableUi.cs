using UnityEngine;
using UnityEngine.UI;
using Utils.Types.Ui;

public class ExpandableUi : MonoBehaviour {
	protected enum ExpandableDirection {
		All        = 0,
		Vertical   = 1,
		Horizontal = 2
	}

	[SerializeField] protected ExpandableDirection   _direction;
	[SerializeField] protected RectTransformPosition _contractedPosition = new RectTransformPosition();
	[SerializeField] protected RectTransformPosition _expandedPosition   = new RectTransformPosition();
	[SerializeField] protected Button                _expandButton;
	[SerializeField] protected Button                _contractButton;
	[SerializeField] protected float                 _smoothness = .5f;
	[SerializeField] protected bool                  _initiallyExpanded;

	private float   resizeLerp           { get; set; }
	private Vector2 destinationOffsetMin { get; set; }
	private Vector2 destinationOffsetMax { get; set; }

	private new RectTransform transform { get; set; }

	private void Awake() {
		transform = GetComponent<RectTransform>();
		_expandButton.onClick.AddListenerOnce(Expand);
		_contractButton.onClick.AddListenerOnce(Contract);
		if (_initiallyExpanded) Expand();
		else Contract();
	}

	private void Contract() {
		SetDestination(_contractedPosition);
		_contractButton.gameObject.SetActive(false);
		_expandButton.gameObject.SetActive(true);
	}

	private void Expand() {
		SetDestination(_expandedPosition);
		_contractButton.gameObject.SetActive(true);
		_expandButton.gameObject.SetActive(false);
	}

	private void SetDestination(RectTransformPosition destination) {
		var destinationAnchorMin = new Vector2(_direction.In(ExpandableDirection.All, ExpandableDirection.Horizontal) ? destination.anchorMin.x : transform.anchorMin.x,
			_direction.In(ExpandableDirection.All, ExpandableDirection.Vertical) ? destination.anchorMin.y : transform.anchorMin.y);
		var destinationAnchorMax = new Vector2(_direction.In(ExpandableDirection.All, ExpandableDirection.Horizontal) ? destination.anchorMax.x : transform.anchorMax.x,
			_direction.In(ExpandableDirection.All, ExpandableDirection.Vertical) ? destination.anchorMax.y : transform.anchorMax.y);
		transform.MoveAnchorsKeepPosition(destinationAnchorMin, destinationAnchorMax);
		destinationOffsetMin = new Vector2(_direction.In(ExpandableDirection.All, ExpandableDirection.Horizontal) ? destination.offset.left : transform.offsetMin.x,
			_direction.In(ExpandableDirection.All, ExpandableDirection.Vertical) ? destination.offset.bottom : transform.offsetMin.y);
		destinationOffsetMax = new Vector2(_direction.In(ExpandableDirection.All, ExpandableDirection.Horizontal) ? destination.offset.right : transform.offsetMax.x,
			_direction.In(ExpandableDirection.All, ExpandableDirection.Vertical) ? destination.offset.top : transform.offsetMax.y);
		if (_smoothness == 0) SetLerp(1);
		else resizeLerp = 0;
	}

	private void Update() {
		if (resizeLerp >= 1) return;
		if (_smoothness == 0) SetLerp(1);
		else SetLerp(resizeLerp + Time.deltaTime / _smoothness);
	}

	private void SetLerp(float lerp) {
		resizeLerp = lerp.Clamp(0, 1);
		transform.offsetMin = Vector2.Lerp(transform.offsetMin, destinationOffsetMin, resizeLerp);
		transform.offsetMax = Vector2.Lerp(transform.offsetMax, destinationOffsetMax, resizeLerp);
	}

	[ContextMenu("Save expanded values")]
	private void SaveExpandedValues() {
		if (!transform) transform = GetComponent<RectTransform>();
		_expandedPosition.anchorMin = transform.anchorMin;
		_expandedPosition.anchorMax = transform.anchorMax;
		var offsetMax = transform.offsetMax;
		var offsetMin = transform.offsetMin;
		_expandedPosition.offset = new RectOffset((int) offsetMin.x, (int) offsetMax.x, (int) offsetMax.y, (int) offsetMin.y);
	}

	[ContextMenu("Save contracted values")]
	private void SaveContractedValues() {
		if (!transform) transform = GetComponent<RectTransform>();
		_contractedPosition.anchorMin = transform.anchorMin;
		_contractedPosition.anchorMax = transform.anchorMax;
		var offsetMax = transform.offsetMax;
		var offsetMin = transform.offsetMin;
		_contractedPosition.offset = new RectOffset((int) offsetMin.x, (int) offsetMax.x, (int) offsetMax.y, (int) offsetMin.y);
	}
}