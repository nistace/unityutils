using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Utils.Ui.Gauges {
	[RequireComponent(typeof(Image))]
	public class GaugeImageUi : MonoBehaviourUi {
		[SerializeField] protected Image _image;

		public float fillAmount {
			get => _image.fillAmount;
			set => _image.fillAmount = value;
		}

		public Color color {
			get => _image.color;
			set => _image.color = value;
		}

		private void Reset() {
			_image = GetComponent<Image>();
		}

		public void SlowlyChangeFill(float targetFill, float delayBeforeStart, float time) {
			StartSingleCoroutine(DoSlowlyChangeFill(targetFill, delayBeforeStart, time));
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
	}
}