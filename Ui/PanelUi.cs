using UnityEngine;

namespace Utils.Ui {
	[RequireComponent(typeof(PanelTransformUi))]
	public class PanelUi : MonoBehaviourUi {
		private PanelTransformUi pt             { get; set; }
		public  PanelTransformUi panelTransform => pt ? pt : pt = GetComponent<PanelTransformUi>();

		public void Move(bool open, float? time = null) {
			if (open) Open(time);
			else Close(time);
		}

		public virtual void Open(float? time = null) => panelTransform.Open(time);
		public virtual void Close(float? time = null) => panelTransform.Close(time);

		public void SetOpen() => Open(0);
		public virtual void SetClose() => Close(0);
	}
}