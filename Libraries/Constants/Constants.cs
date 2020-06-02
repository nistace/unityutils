public static class Constants {
	private static ConstantLibrary library { get; set; }
	public static  bool            loaded  => library != null;

	public static void LoadLibrary(ConstantLibrary libraryToLoad) {
		library = libraryToLoad;
		if (library) library.Load();
	}

	public static int Int(string name, int defaultValue = 0) => int.TryParse(String(name), out var value) ? value : defaultValue;
	public static float Float(string name, float defaultValue = 0) => float.TryParse(String(name).Replace('.', ','), out var value) ? value : defaultValue;
	public static bool Bool(string name, bool defaultValue = false) => bool.TryParse(String(name), out var value) ? value : defaultValue;

	public static string String(string name) => library?[name] ?? string.Empty;
}