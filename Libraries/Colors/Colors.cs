using System.Collections.Generic;
using UnityEngine;

public static class Colors {
	private static ColorLibrary library { get; set; }
	public static  bool         loaded  => library != null;

	public static Color white            { get; } = new Color(1, 1, 1, 1);
	public static Color transparentWhite { get; } = new Color(1, 1, 1, 0);
	public static Color black            { get; } = new Color(0, 0, 0, 1);
	public static Color red              { get; } = new Color(1, 0, 0, 1);
	public static Color none             { get; } = new Color(0, 0, 0, 0);

	public static void LoadLibrary(ColorLibrary libraryToLoad) {
		library = libraryToLoad;
		if (library) library.Load();
	}

	public static Color Of(string paletteColorName, float? r = null, float? g = null, float? b = null, float? a = null) {
		return Of(library?[paletteColorName] ?? Color.black, r, g, b, a);
	}

	public static Color Of(Color c, float? r = null, float? g = null, float? b = null, float? a = null) {
		return Of(r ?? c.r, g ?? c.g, b ?? c.b, a ?? c.a);
	}

	public static Color Of(float r, float g, float b, float a) {
		return new Color(r, g, b, a);
	}

	public static Color Random(string keyRoot) => library.GetRandom(keyRoot);
	public static string RandomKey(string keyRoot) => library.GetRandomKey(keyRoot);
	public static bool HasKey(string key) => library.HasKey(key);
	public static IReadOnlyDictionary<string, Color> AllStartingWith(string keyRoot) => library.AllStartingWith(keyRoot);
}