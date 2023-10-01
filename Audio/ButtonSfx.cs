using NiUtils.Extensions;
using NiUtils.Libraries;
using UnityEngine;
using UnityEngine.UI;

namespace NiUtils.Audio {
	public class ButtonSfx : MonoBehaviour {
		[SerializeField] protected Button _button;
		[SerializeField] protected string _clipKey;

		private void Reset() => _button = GetComponent<Button>();

		private void Start() => _button.onClick.AddListenerOnce(PlayClip);

		private void PlayClip() => AudioManager.Sfx.Play(AudioClips.Of(_clipKey));
	}
}