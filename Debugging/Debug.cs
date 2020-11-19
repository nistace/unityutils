﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils.Events;

namespace Utils.Debugging {
	public static class Debug {
		public static BoolEvent onDebugDisplayedChanged => DebugUi.onDisplayedChanged;

		public enum Type {
			Info    = 0,
			Error   = 1,
			Warning = 2,
			Cheat   = 3
		}

		private static Dictionary<Type, Color> colors { get; } = new Dictionary<Type, Color> {{Type.Info, Color.white}, {Type.Error, Color.red}, {Type.Warning, Color.yellow}, {Type.Cheat, Color.cyan}};

		public static void SetColor(Type type, Color color) {
			colors.Set(type, color);
		}

		public static void Log(string info) => Log(Type.Info, info);

		public static void LogError(string info) => Log(Type.Error, info, UnityEngine.Debug.LogError);

		public static void LogWarning(string info) => Log(Type.Warning, info, UnityEngine.Debug.LogWarning);

		public static void LogCheat(string info) => Log(Type.Cheat, info);

		private static void Log(Type type, string info, Action<object> consoleLogAction = null) {
			(consoleLogAction ?? UnityEngine.Debug.Log)($"[{type.ToString().ToUpper()}] {info}");
			DebugUi.Print(info, type.ToString().ToUpper(), colors.Of(type, Color.white));
		}
	}
}