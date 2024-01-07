using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NiUtils.Coroutines;
using NiUtils.Extensions;
using NiUtils.Libraries;
using NiUtils.Types;
using UnityEngine;

namespace NiUtils.Audio {
	public class AudioManager : MonoBehaviour {
		[SerializeField] protected float _changeMusicClipSpeed = 1;

		[SerializeField]                           protected Ratio        _masterVolume = 1;
		[Header("Music")] [SerializeField]         protected AudioSource  _musicSource;
		[SerializeField]                           protected Ratio        _musicVolume        = 1;
		[SerializeField]                           protected bool         _autoSelectNextClip = true;
		[Header("Sound effects")] [SerializeField] protected Ratio        _sfxVolume          = 1;
		[Header("Voices")] [SerializeField]        protected Ratio        _voicesVolume       = 1;
		private static                                       AudioManager instance { get; set; }

		private void Reset() {
			if (!_musicSource) _musicSource = this.GetOrAddComponent<AudioSource>();
		}

		private Queue<AudioSource> availableSources { get; } = new Queue<AudioSource>();

		public static Ratio masterVolume {
			get => instance._masterVolume;
			set {
				instance._masterVolume = Mathf.Clamp01(value);
				Sfx.volume = Sfx.volume;
				Music.volume = Music.volume;
				Voices.volume = Voices.volume;
			}
		}

		public void Awake() {
			if (instance == null) instance = this;
			if (instance != this) Destroy(gameObject);
			else masterVolume = masterVolume;
		}

		public void Update() {
			Sfx.Update();
			Voices.Update();
			Music.Update();
		}

		private AudioSource CreateNewSource() {
			var sfxSource = new GameObject("Source").ParentedTo(transform).AddComponent<AudioSource>();
			sfxSource.loop = false;
			return sfxSource;
		}

		private void OnValidate() {
			if (!instance) return;
			Sfx.volume = Sfx.volume;
			Music.volume = Music.volume;
			Voices.volume = Voices.volume;
		}

		public static class Sfx {
			private static List<AudioSource> playingSources { get; } = new List<AudioSource>();

			public static float volume {
				get => instance._sfxVolume;
				set {
					instance._sfxVolume = value;
					playingSources.ForEach(t => t.volume = value * instance._masterVolume);
				}
			}

			public static AudioSource Play(string audioClipKey, float volumeCoefficient = 1, Transform source = null) => Play(AudioClips.Of(audioClipKey), volumeCoefficient, source);
			public static AudioSource PlayRandom(string audioClipKeyRoot, float volumeCoefficient = 1, Transform source = null) => Play(AudioClips.RandomOf(audioClipKeyRoot), volumeCoefficient, source);

			public static AudioSource Play(AudioClip clip, float volumeCoefficient = 1, Transform source = null) {
				var src = instance.availableSources.Count > 0 ? instance.availableSources.Dequeue() : instance.CreateNewSource();
				src.loop = false;
				src.volume = volume * instance._masterVolume * volumeCoefficient;
				src.clip = clip;
				src.gameObject.Active().transform.ParentedTo(source).ResetLocalAttributes();
				src.Play();
				playingSources.Add(src);
				return src;
			}

			internal static void Update() {
				if (playingSources.Count == 0) return;
				for (var i = 0; i < playingSources.Count; ++i) {
					if (!playingSources[i]) {
						playingSources.RemoveAt(i);
						i--;
					}
					else if (!playingSources[i].isPlaying) {
						instance.availableSources.Enqueue(playingSources[i]);
						playingSources[i].gameObject.Inactive().transform.ParentedTo(instance.transform).ResetLocalAttributes();
						playingSources.Remove(playingSources[i]);
						i--;
					}
				}
			}
		}

		public static class Voices {
			private static HashSet<AudioSource> playingSources { get; } = new HashSet<AudioSource>();

			public static float volume {
				get => instance._voicesVolume;
				set {
					instance._voicesVolume = value;
					playingSources.ForEach(t => t.volume = value * instance._masterVolume);
				}
			}

			public static AudioSource Play(string audioClipKey) => Play(AudioClips.Of(audioClipKey));
			public static AudioSource PlayRandom(string audioClipKeyRoot) => Play(AudioClips.RandomOf(audioClipKeyRoot));

			public static AudioSource Play(AudioClip clip) {
				var src = instance.availableSources.Count > 0 ? instance.availableSources.Dequeue() : instance.CreateNewSource();
				src.volume = volume * instance._masterVolume;
				src.clip = clip;
				src.gameObject.SetActive(true);
				src.Play();
				playingSources.Add(src);
				return src;
			}

			internal static void Update() {
				if (playingSources.Count == 0 || playingSources.All(t => t.isPlaying)) return;
				foreach (var finishedSfx in playingSources.Where(t => !t.isPlaying).ToArray()) {
					playingSources.Remove(finishedSfx);
					instance.availableSources.Enqueue(finishedSfx);
					finishedSfx.transform.SetParent(instance.transform);
					finishedSfx.gameObject.SetActive(false);
				}
			}
		}

		public static class Music {
			public static float volume {
				get => instance._musicVolume;
				set {
					instance._musicVolume = value;
					instance._musicSource.volume = value * instance._masterVolume;
				}
			}

			public static float timeInCurrentClip => instance._musicSource.time;

			public static bool autoSelectNextClip {
				get => instance._autoSelectNextClip;
				set => instance._autoSelectNextClip = value;
			}

			private static float tmpVolume {
				get => (instance._musicSource?.volume ?? 0) / instance._masterVolume;
				set => instance._musicSource.volume = value * instance._masterVolume;
			}

			public static bool loop {
				get => instance._musicSource.loop;
				set => instance._musicSource.loop = value;
			}

			private static SingleCoroutine changeClipRoutine { get; set; }

			public static bool isPlaying => instance._musicSource.isPlaying;

			public static void ChangeToRandomClip(string clipKeyRoot, bool instantly, bool restartIfPlaying = false) => ChangeClip(AudioClips.RandomOf(clipKeyRoot), instantly, restartIfPlaying);
			public static void ChangeClip(string clipKey, bool instantly, bool restartIfPlaying = false) => ChangeClip(AudioClips.Of(clipKey), instantly, restartIfPlaying);

			public static void ChangeClip(AudioClip clip, bool instantly, bool restartIfPlaying = false) {
				if (changeClipRoutine == null) changeClipRoutine = new SingleCoroutine(instance);
				changeClipRoutine.Start(DoChangeClip(clip, instantly, restartIfPlaying));
			}

			private static IEnumerator DoChangeClip(AudioClip clip, bool instantly, bool restartIfPlaying = false) {
				if (instance._musicSource.clip == clip && tmpVolume == volume && !restartIfPlaying) yield break;
				if (instance._musicSource.clip && (instance._musicSource.clip != clip || restartIfPlaying) && instance._musicSource.isPlaying) {
					if (instantly) tmpVolume = 0;
					while (tmpVolume > 0) {
						tmpVolume -= Time.deltaTime * instance._changeMusicClipSpeed;
						yield return null;
					}
					tmpVolume = 0;
				}
				instance._musicSource.clip = clip;
				instance._musicSource.Play();
				while (!instantly && tmpVolume < volume) {
					tmpVolume += Time.deltaTime * instance._changeMusicClipSpeed;
					yield return null;
				}
				tmpVolume = volume;
			}

			public static void Update() {
				if (autoSelectNextClip && !isPlaying) ChangeToRandomClip("music", false);
			}
		}
	}
}