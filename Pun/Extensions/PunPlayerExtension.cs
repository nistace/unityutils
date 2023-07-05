using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;

namespace NiUtils.Pun.Extensions {
	public static class PunPlayerExtension {
		public static void SetCustomProperty(this Player player, string key, int value) => player.SetCustomProperty(t => t.Define(key, value));
		public static void SetCustomProperty(this Player player, string key, string value) => player.SetCustomProperty(t => t.Define(key, value));
		public static void SetCustomProperty(this Player player, string key, float value) => player.SetCustomProperty(t => t.Define(key, value));
		public static void SetCustomProperty(this Player player, string key, bool value) => player.SetCustomProperty(t => t.Define(key, value));
		public static void SetCustomProperty(this Player player, string key, IEnumerable<int> value) => player.SetCustomProperty(t => t.Define(key, value));
		public static void SetCustomProperty(this Player player, string key, IEnumerable<float> value) => player.SetCustomProperty(t => t.Define(key, value));
		public static void SetCustomProperty(this Player player, string key, Vector2 value) => player.SetCustomProperty(t => t.Define(key, value));
		public static void SetCustomProperty(this Player player, string key, Vector3 value) => player.SetCustomProperty(t => t.Define(key, value));
		public static void SetCustomProperty(this Player player, string key, Color value) => player.SetCustomProperty(t => t.Define(key, value));

		private static void SetCustomProperty(this Player player, Action<Hashtable> defineFunc) {
			var data = new Hashtable();
			defineFunc(data);
			player.SetCustomProperties(data);
		}
	}
}