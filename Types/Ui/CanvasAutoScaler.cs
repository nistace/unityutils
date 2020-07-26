using UnityEngine;
using UnityEngine.UI;

public class CanvasAutoScaler : MonoBehaviour {
	[SerializeField] protected CanvasScaler _scaler;
	[SerializeField] protected float        _minScaleFactor = 1;
	[SerializeField] protected float        _maxScaleFactor = 2;
	[SerializeField] protected Vector2      _minScreenSize  = new Vector2(1920, 1080);
	[SerializeField] protected Vector2      _maxScreenSize  = new Vector2(3840, 2160);

	private float previousWidth  { get; set; }
	private float previousHeight { get; set; }

	private void Reset() {
		_scaler = GetComponent<CanvasScaler>();
	}

	private void Start() {
		Refresh();
	}

	private void Update() {
		if (previousWidth == Display.main.renderingWidth && previousHeight == Display.main.renderingHeight) return;
		Refresh();
	}

	private void Refresh() {
		previousWidth = Display.main.renderingWidth;
		previousHeight = Display.main.renderingHeight;
		var widthLerp = (previousWidth - _minScreenSize.x) / (_maxScreenSize.x - _minScreenSize.x);
		var heightLerp = (previousHeight - _minScreenSize.y) / (_maxScreenSize.y - _minScreenSize.y);
		_scaler.scaleFactor = Mathf.Lerp(_minScaleFactor, _maxScaleFactor, Mathf.Min(widthLerp, heightLerp).Clamp(0, 1));
	}
}