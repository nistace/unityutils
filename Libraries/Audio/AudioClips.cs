using UnityEngine;

namespace Utils.Libraries {
	public static class AudioClips {
		private static AudioClipLibrary library { get; set; }
		public static  bool             loaded  => library != null;

		public static void LoadLibrary(AudioClipLibrary libraryToLoad) {
			library = libraryToLoad;
			if (library) library.Load();
		}

		public static AudioClip Of(string clipName) {
			return library?[clipName];
		}

		public static AudioClip RandomOf(string clipRootName) => library.GetRandom(clipRootName);

		public static bool HasKey(string key) => library.HasKey(key);
	}
}