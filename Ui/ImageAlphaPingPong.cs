using NiUtils.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace NiUtils.Ui {
	[RequireComponent(typeof(Image))]
	public class ImageAlphaPingPong : MonoBehaviour {
		[SerializeField] protected Image _image;
		[SerializeField] protected float _minAlpha;
		[SerializeField] protected float _maxAlpha = 1;
		[SerializeField] protected float _speed    = 1;

		private float lerp          { get; set; }
		private float lerpDirection { get; set; } = 1;

		private void Reset() {
			_image = GetComponent<Image>();
		}

		private void Update() {
			lerp += lerpDirection * Time.deltaTime * _speed;
			if (lerp <= 0) lerpDirection = 1;
			else if (lerp >= 1) lerpDirection = -1;
			_image.color = _image.color.With(a: Mathf.Lerp(_minAlpha, _maxAlpha, lerp));
		}
	}
}