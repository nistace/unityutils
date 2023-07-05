using UnityEngine;
using UnityEngine.UI;

namespace NiUtils.Ui {
	[RequireComponent(typeof(Image))]
	[RequireComponent(typeof(Button))]
	public class ImageButton : MonoBehaviour {
		[SerializeField] protected Image  _image;
		[SerializeField] protected Button _button;
	}
}