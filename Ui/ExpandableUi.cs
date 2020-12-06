using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils.Types.Ui;
using Utils.Ui;

public class ExpandableUi : MonoBehaviourUi {
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
	[SerializeField] protected float                 _defaultSmoothness = .5f;
	[SerializeField] protected bool                  _initiallyExpanded;

	private void Awake() {
		_expandButton.onClick.AddListenerOnce(Expand);
		_contractButton.onClick.AddListenerOnce(Contract);
		if (_initiallyExpanded) Expand(0);
		else Contract(0);
	}

	private void Contract() => Contract(null);

	private void Contract(float? smoothness) {
		MoveTo(_contractedPosition, smoothness ?? _defaultSmoothness);
		_contractButton.gameObject.SetActive(false);
		_expandButton.gameObject.SetActive(true);
	}

	private void Expand() => Expand(null);

	private void Expand(float? smoothness) {
		MoveTo(_expandedPosition, smoothness ?? _defaultSmoothness);
		_contractButton.gameObject.SetActive(true);
		_expandButton.gameObject.SetActive(false);
	}

	private void MoveTo(RectTransformPosition destination, float smoothness) {
		if (smoothness <= 0) JumpTo(destination);
		else StartSingleCoroutine(DoMovement(destination, smoothness));
	}

	private void JumpTo(RectTransformPosition destination) {
		transform.MoveAnchorsKeepPosition(TransformWithDirection(destination.anchorMin, transform.anchorMin), TransformWithDirection(destination.anchorMax, transform.anchorMax));
		transform.offsetMin = TransformWithDirection(destination.offsetMin, transform.offsetMin);
		transform.offsetMax = TransformWithDirection(destination.offsetMax, transform.offsetMax);
	}

	private IEnumerator DoMovement(RectTransformPosition destination, float smoothness) {
		var destinationOffsetMin = TransformWithDirection(destination.offsetMin, transform.offsetMin);
		var destinationOffsetMax = TransformWithDirection(destination.offsetMax, transform.offsetMax);
		transform.MoveAnchorsKeepPosition(TransformWithDirection(destination.anchorMin, transform.anchorMin), TransformWithDirection(destination.anchorMax, transform.anchorMax));

		if (smoothness > 0) {
			var timeCoefficient = 1 / smoothness;
			for (var lerp = 0f; lerp <= 1; lerp += Time.deltaTime * timeCoefficient) {
				transform.offsetMin = Vector2.Lerp(transform.offsetMin, destinationOffsetMin, lerp);
				transform.offsetMax = Vector2.Lerp(transform.offsetMax, destinationOffsetMax, lerp);
				yield return null;
			}
		}
		transform.offsetMin = destinationOffsetMin;
		transform.offsetMax = destinationOffsetMax;
	}

	private Vector2 TransformWithDirection(Vector2 expected, Vector2 current) {
		switch (_direction) {
			case ExpandableDirection.All: return expected;
			case ExpandableDirection.Horizontal: return expected.With(y: current.y);
			case ExpandableDirection.Vertical: return expected.With(x: current.x);
			default: throw new ArgumentOutOfRangeException();
		}
	}

#if UNITY_EDITOR

	private void SaveCurrent(RectTransformPosition inPosition) {
		inPosition.anchorMin = transform.anchorMin;
		inPosition.anchorMax = transform.anchorMax;
		inPosition.offsetMax = transform.offsetMax;
		inPosition.offsetMin = transform.offsetMin;
	}

	[ContextMenu("Save expanded values")] private void SaveExpandedValues() => SaveCurrent(_expandedPosition);
	[ContextMenu("Save contracted values")] private void SaveContractedValues() => SaveCurrent(_contractedPosition);
#endif
}