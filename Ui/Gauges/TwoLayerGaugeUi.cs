using UnityEngine;

namespace Utils.Ui.Gauges {
	public class TwoLayerGaugeUi : MonoBehaviourUi {
		[SerializeField] protected GaugeImageUi _frontGauge;
		[SerializeField] protected GaugeImageUi _backGauge;

		public GaugeImageUi frontGauge => _frontGauge;
		public GaugeImageUi backGauge  => _backGauge;

		public float fillAmount {
			get => _frontGauge.fillAmount;
			set {
				_backGauge.fillAmount = value;
				_frontGauge.fillAmount = value;
			}
		}

		public void SlowlyChangeFill(float targetFill, float delayBeforeStart, float time) {
			if (_frontGauge.fillAmount == targetFill) return;
			if (_frontGauge.fillAmount > targetFill) {
				_backGauge.fillAmount = _frontGauge.fillAmount;
				_frontGauge.fillAmount = targetFill;
				_backGauge.SlowlyChangeFill(targetFill, delayBeforeStart, time);
			}
			else {
				_backGauge.fillAmount = targetFill;
				_frontGauge.SlowlyChangeFill(targetFill, delayBeforeStart, time);
			}
		}
	}
}