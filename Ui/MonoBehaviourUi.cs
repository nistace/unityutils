using UnityEngine;

namespace NiUtils.Ui {
	[RequireComponent(typeof(RectTransform))]
	public class MonoBehaviourUi : MonoBehaviour {
		private    RectTransform rt        { get; set; }
		public new RectTransform transform => rt ? rt : rt = GetComponent<RectTransform>();
	}
}