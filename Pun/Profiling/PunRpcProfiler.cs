#if PHOTON_UNITY_NETWORKING
using System.Collections.Generic;
using System.Text;
using NiUtils.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NiUtils.Pun.Profiling {
	public class PunRpcProfiler : MonoBehaviour {
		private static PunRpcProfiler instance         { get; set; }
		public static  bool           exists           => instance;
		public static  bool           displayPerFrame  { get; set; } = true;
		public static  bool           displayPerSecond { get; set; } = true;

		public static void Instantiate() {
			if (instance) return;
			instance = new GameObject("PunRpcProfiler").AddComponent<PunRpcProfiler>();
			DontDestroyOnLoad(instance.gameObject);
		}

		public static void Destroy() {
			if (!instance) return;
			Object.Destroy(instance.gameObject);
		}

		private int           second        { get; set; }
		private Counter       frameCounter  { get; } = new Counter();
		private Counter       secondCounter { get; } = new Counter();
		private StringBuilder logBuilder    { get; } = new StringBuilder();

		private void Update() {
			if (frameCounter.hasAnything) {
				if (displayPerFrame) {
					logBuilder.Clear();
					logBuilder.Append("Profiling frame count");
					frameCounter.Fill(logBuilder);
					Debug.Log(logBuilder.ToString());
				}
				frameCounter.Clear();
			}
			if ((int)Time.time == second) return;
			if (secondCounter.hasAnything) {
				if (displayPerSecond) {
					logBuilder.Clear();
					logBuilder.Append("Profiling second count");
					secondCounter.Fill(logBuilder);
				}
				Debug.Log(logBuilder.ToString());
			}
			second = (int)Time.time;
			secondCounter.Clear();
		}

		public static void AddRpcSent(string functionName) {
			if (!instance) return;
			instance.frameCounter.AddSent(functionName);
			instance.secondCounter.AddSent(functionName);
		}

		public static void AddRpcReceived(string functionName) {
			if (!instance) return;
			instance.frameCounter.AddReceived(functionName);
			instance.secondCounter.AddReceived(functionName);
		}

		private class Counter {
			private int                     sentCount               { get; set; }
			private int                     receivedCount           { get; set; }
			private Dictionary<string, int> sentPerFunctionName     { get; } = new Dictionary<string, int>();
			private Dictionary<string, int> receivedPerFunctionName { get; } = new Dictionary<string, int>();

			public bool hasAnything => sentCount > 0 || receivedCount > 0;

			public void Clear() {
				sentPerFunctionName.Clear();
				receivedPerFunctionName.Clear();
				sentCount = 0;
				receivedCount = 0;
			}

			public void Fill(StringBuilder stringBuilder) {
				if (sentCount > 0) stringBuilder.Append(" SENT:").Append(sentCount).Append("(").Append(sentPerFunctionName.FirstWhereMaxOrDefault(t => t.Value)).Append(")");
				if (receivedCount > 0) stringBuilder.Append(" RECEIVED:").Append(receivedCount).Append("(").Append(receivedPerFunctionName.FirstWhereMaxOrDefault(t => t.Value)).Append(")");
			}

			public void AddSent(string functionName) {
				sentCount++;
				if (!sentPerFunctionName.ContainsKey(functionName)) sentPerFunctionName.Add(functionName, 0);
				sentPerFunctionName[functionName]++;
			}

			public void AddReceived(string functionName) {
				receivedCount++;
				if (!receivedPerFunctionName.ContainsKey(functionName)) receivedPerFunctionName.Add(functionName, 0);
				receivedPerFunctionName[functionName]++;
			}
		}
	}
}
#endif