using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;

namespace Utils.Extensions {
	public static class PunRoomExtension {
		public static void SetCustomProperty(this Room room, string key, int value) => room.SetCustomProperty(t => t.Define(key, value));
		public static void SetCustomProperty(this Room room, string key, string value) => room.SetCustomProperty(t => t.Define(key, value));
		public static void SetCustomProperty(this Room room, string key, float value) => room.SetCustomProperty(t => t.Define(key, value));
		public static void SetCustomProperty(this Room room, string key, bool value) => room.SetCustomProperty(t => t.Define(key, value));
		public static void SetCustomProperty(this Room room, string key, IEnumerable<int> value) => room.SetCustomProperty(t => t.Define(key, value));
		public static void SetCustomProperty(this Room room, string key, IEnumerable<float> value) => room.SetCustomProperty(t => t.Define(key, value));
		public static void SetCustomProperty(this Room room, string key, Vector2 value) => room.SetCustomProperty(t => t.Define(key, value));
		public static void SetCustomProperty(this Room room, string key, Vector3 value) => room.SetCustomProperty(t => t.Define(key, value));
		public static void SetCustomProperty(this Room room, string key, Color value) => room.SetCustomProperty(t => t.Define(key, value));

		private static void SetCustomProperty(this Room room, Action<Hashtable> defineFunc) {
			var data = new Hashtable();
			defineFunc(data);
			room.SetCustomProperties(data);
		}
	}
}