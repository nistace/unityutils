using System;
using UnityEngine;
using UnityEngine.UI;

namespace Utils.Audio {
	public class ButtonSfx : MonoBehaviour {
		[SerializeField] protected Button _button;
		[SerializeField] protected string _clipKey;

		private void Reset() => _button = GetComponent<Button>();

		private void Awake() => _button.onClick.AddListenerOnce(PlayClip);

		private void PlayClip() => AudioManager.Sfx.Play(AudioClips.Of(_clipKey));
	}
}