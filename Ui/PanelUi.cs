using UnityEngine;

namespace Utils.Ui {
	[RequireComponent(typeof(PanelTransformUi))]
	public abstract class PanelUi : MonoBehaviourUi {
		private PanelTransformUi pt             { get; set; }
		public  PanelTransformUi panelTransform => pt ? pt : pt = GetComponent<PanelTransformUi>();
	}
}