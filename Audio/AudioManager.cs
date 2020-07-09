using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils.Audio {
	public class AudioManager : MonoBehaviour {
		private static AudioManager instance { get; set; }

		[SerializeField]                           protected Ratio       _masterVolume = 1;
		[Header("Music")] [SerializeField]         protected AudioSource _musicSource;
		[SerializeField]                           protected Ratio       _musicVolume          = 1;
		[SerializeField]                           protected float       _changeMusicClipSpeed = 1;
		[Header("Sound effects")] [SerializeField] protected Ratio       _sfxVolume            = 1;
		[Header("Voices")] [SerializeField]        protected Ratio       _voicesVolume         = 1;

		private Queue<AudioSource> availableSources { get; } = new Queue<AudioSource>();

		public static Ratio masterVolume {
			get => instance._masterVolume;
			set {
				instance._masterVolume = value;
				Sfx.volume = Sfx.volume;
				Music.volume = Music.volume;
				Voices.volume = Voices.volume;
			}
		}

		public void Awake() {
			if (instance == null) instance = this;
			if (instance != this) Destroy(gameObject);
		}

		public void Update() {
			Sfx.Update();
			Voices.Update();
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
			private static HashSet<AudioSource> playingSources { get; } = new HashSet<AudioSource>();

			public static float volume {
				get => instance._sfxVolume;
				set {
					instance._sfxVolume = value;
					playingSources.ForEach(t => t.volume = value * instance._masterVolume);
				}
			}

			public static void Play(string audioClipKey) => Play(AudioClips.Of(audioClipKey));
			public static void PlayRandom(string audioClipKeyRoot) => Play(AudioClips.RandomOf(audioClipKeyRoot));

			public static void Play(AudioClip clip) => Play(clip, 1);

			public static void Play(AudioClip clip, float volumeCoefficient) {
				var src = instance.availableSources.Count > 0 ? instance.availableSources.Dequeue() : instance.CreateNewSource();
				src.volume = volume * instance._masterVolume * volumeCoefficient;
				src.clip = clip;
				src.gameObject.SetActive(true);
				src.Play();
				playingSources.Add(src);
			}

			internal static void Update() {
				if (playingSources.Count == 0) return;
				foreach (var finishedSfx in playingSources.Where(t => !t.isPlaying).ToArray()) {
					playingSources.Remove(finishedSfx);
					instance.availableSources.Enqueue(finishedSfx);
					finishedSfx.gameObject.SetActive(false);
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

			public static void Play(string audioClipKey) => Play(AudioClips.Of(audioClipKey));
			public static void PlayRandom(string audioClipKeyRoot) => Play(AudioClips.RandomOf(audioClipKeyRoot));

			public static void Play(AudioClip clip) {
				var src = instance.availableSources.Count > 0 ? instance.availableSources.Dequeue() : instance.CreateNewSource();
				src.volume = volume * instance._masterVolume;
				src.clip = clip;
				src.gameObject.SetActive(true);
				src.Play();
				playingSources.Add(src);
			}

			internal static void Update() {
				if (playingSources.Count == 0 || playingSources.All(t => t.isPlaying)) return;
				foreach (var finishedSfx in playingSources.Where(t => !t.isPlaying).ToArray()) {
					playingSources.Remove(finishedSfx);
					instance.availableSources.Enqueue(finishedSfx);
					finishedSfx.gameObject.SetActive(false);
				}
			}
		}

		public static class Music {
			public static float volume {
				get => instance._musicVolume;
				set {
					instance._musicVolume = value;
					if (instance._musicSource) instance._musicSource.volume = value * instance._masterVolume;
				}
			}

			public static bool loop {
				get => instance._musicSource.loop;
				set => instance._musicSource.loop = value;
			}

			public static bool isPlaying => instance._musicSource.isPlaying;

			public static void ChangeToRandomClip(string clipKeyRoot, bool restartIfPlaying = false) => ChangeClip(AudioClips.RandomOf(clipKeyRoot), restartIfPlaying);
			public static void ChangeClip(string clipKey, bool restartIfPlaying = false) => ChangeClip(AudioClips.Of(clipKey), restartIfPlaying);

			public static void ChangeClip(AudioClip clip, bool restartIfPlaying = false) {
				instance.StartCoroutine(DoChangeClip(clip, restartIfPlaying));
			}

			private static IEnumerator DoChangeClip(AudioClip clip, bool restartIfPlaying = false) {
				if (instance._musicSource.clip == clip && !restartIfPlaying) yield break;
				var targetVolume = volume;
				if (instance._musicSource.clip && instance._musicSource.isPlaying) {
					while (volume > 0) {
						volume -= Time.deltaTime * instance._changeMusicClipSpeed;
						yield return null;
					}
				}
				volume = 0;
				instance._musicSource.clip = clip;
				instance._musicSource.Play();
				while (volume < targetVolume) {
					volume += Time.deltaTime * instance._changeMusicClipSpeed;
					yield return null;
				}
				volume = targetVolume;
			}
		}
	}
}