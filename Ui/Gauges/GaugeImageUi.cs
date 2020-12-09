using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils.Coroutines;

namespace Utils.Ui.Gauges {
	[RequireComponent(typeof(Image))]
	public class GaugeImageUi : MonoBehaviourUi {
		[SerializeField] protected Image _image;

		public  Image           image           => _image;
		private SingleCoroutine singleCoroutine { get; set; }

		public float fillAmount {
			get => _image.fillAmount;
			private set => _image.fillAmount = value;
		}

		public Color color {
			get => _image.color;
			set => _image.color = value;
		}

		private void Awake() {
			singleCoroutine = new SingleCoroutine(this);
		}

		private void Reset() {
			_image = GetComponent<Image>();
		}

		public void SlowlyChangeFill(float targetFill, float delayBeforeStart, float time) {
			if (!gameObject.activeInHierarchy || singleCoroutine == null || delayBeforeStart <= 0 && time <= 0) fillAmount = targetFill;
			else singleCoroutine.Start(DoSlowlyChangeFill(targetFill, delayBeforeStart, time));
		}

		private IEnumerator DoSlowlyChangeFill(float targetFill, float delayBeforeStart, float time) {
			for (var delayProgress = 0f; delayProgress < delayBeforeStart; delayProgress += Time.deltaTime) yield return null;
			if (time > 0) {
				var stepToTarget = Mathf.Abs(targetFill - fillAmount) / time;
				while (fillAmount != targetFill) {
					fillAmount = Mathf.MoveTowards(fillAmount, targetFill, stepToTarget * Time.deltaTime);
					yield return null;
				}
			}
			fillAmount = targetFill;
		}

		public void SetFillAmount(float fill) {
			singleCoroutine?.Stop();
			fillAmount = fill;
		}
	}
}