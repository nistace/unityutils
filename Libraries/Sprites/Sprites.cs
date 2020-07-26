using UnityEngine;

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
}