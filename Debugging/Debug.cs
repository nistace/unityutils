namespace Utils.Debugging {
	public static class Debug {
		public static void Log(string info) {
			UnityEngine.Debug.Log(info);
			DebugUi.Print(info);
		}
	}
}