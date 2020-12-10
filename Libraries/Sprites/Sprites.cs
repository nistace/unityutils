using System.Collections.Generic;
using UnityEngine;

namespace Utils.Libraries {
	public static class Sprites {
		private static SpriteLibrary library { get; set; }
		public static  bool          loaded  => library != null;

		public static void LoadLibrary(SpriteLibrary libraryToLoad) {
			library = libraryToLoad;
			if (library) library.Load();
		}

		public static Sprite Of(string name) {
			return library?[name];
		}

		public static Sprite Random(string keyRoot) => library.GetRandom(keyRoot);
		public static string RandomKey(string keyRoot) => library.GetRandomKey(keyRoot);
		public static bool HasKey(string key) => library.HasKey(key);

		public static IReadOnlyDictionary<string, Sprite> AllStartingWith(string keyRoot) => library.AllStartingWith(keyRoot);
	}
}