using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Utils.Ui {
	public class CanvasAutoScaler : MonoBehaviour {
		[SerializeField] protected CanvasScaler _scaler;
		[SerializeField] protected float        _minScaleFactor    = 1;
		[SerializeField] protected float        _maxScaleFactor    = 2;
		[SerializeField] protected Vector2      _madeForScreenSize = new Vector2(1920, 1080);

		private float previousWidth  { get; set; }
		private float previousHeight { get; set; }

		private void Reset() => _scaler = GetComponent<CanvasScaler>();
		private void Start() => Refresh();

		private void Update() {
			if (previousWidth == Display.main.renderingWidth && previousHeight == Display.main.renderingHeight) return;
			Refresh();
		}

		[ContextMenu("Refresh")]
		private void Refresh() {
			previousWidth = Display.main.renderingWidth;
			previousHeight = Display.main.renderingHeight;
			var widthRatio = previousWidth / _madeForScreenSize.x;
			var heightRatio = previousHeight / _madeForScreenSize.y;
			_scaler.scaleFactor = Mathf.Min(widthRatio, heightRatio).Clamp(_minScaleFactor, _maxScaleFactor);
		}
	}
}