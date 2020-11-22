using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class ImageButton : MonoBehaviour {
	[SerializeField] protected Image  _image;
	[SerializeField] protected Button _button;
}