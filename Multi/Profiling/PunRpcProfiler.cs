using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Utils.Extensions;
using Debug = Utils.Debugging.Debug;
using Object = UnityEngine.Object;

namespace Utils.Multi.Profiling {
	public class PunRpcProfiler : MonoBehaviour {
		private static PunRpcProfiler instance { get; set; }
		public static  bool           exists   => instance;

		public static void Instantiate() {
			if (instance) return;
			instance = new GameObject("PunRpcProfiler").AddComponent<PunRpcProfiler>();
			DontDestroyOnLoad(instance.gameObject);
		}

		public static void Destroy() {
			if (!instance) return;
			Object.Destroy(instance.gameObject);
		}

		private int           frameRpcSentCount     { get; set; }
		private int           frameRpcReceivedCount { get; set; }
		private StringBuilder logBuilder            { get; } = new StringBuilder();

		private Dictionary<string, int> rpcSentPerFunctionName     { get; } = new Dictionary<string, int>();
		private Dictionary<string, int> rpcReceivedPerFunctionName { get; } = new Dictionary<string, int>();

		private void OnEnable() => Clear();

		private void Clear() {
			rpcSentPerFunctionName.Clear();
			rpcReceivedPerFunctionName.Clear();
			frameRpcReceivedCount = 0;
			frameRpcSentCount = 0;
		}

		private void Update() {
			if (frameRpcSentCount > 0 || frameRpcReceivedCount > 0) {
				logBuilder.Clear();
				logBuilder.Append("Profiling frame count");
				if (frameRpcSentCount > 0)
					logBuilder.Append(" SENT:").Append(frameRpcSentCount).Append("(").Append(rpcSentPerFunctionName.FirstWhereMaxOrDefault(t => t.Value)).Append(")");
				if (frameRpcReceivedCount > 0)
					logBuilder.Append(" RECEIVED:").Append(frameRpcReceivedCount).Append("(").Append(rpcReceivedPerFunctionName.FirstWhereMaxOrDefault(t => t.Value)).Append(")");
				Debug.Log(logBuilder.ToString());
			}
			Clear();
		}

		public static void AddRpcSent(string functionName) {
			if (!instance) return;
			instance.frameRpcSentCount++;
			if (!instance.rpcSentPerFunctionName.ContainsKey(functionName)) instance.rpcSentPerFunctionName.Add(functionName, 0);
			instance.rpcSentPerFunctionName[functionName]++;
		}

		public static void AddRpcReceived(string functionName) {
			if (!instance) return;
			instance.frameRpcReceivedCount++;
			if (!instance.rpcReceivedPerFunctionName.ContainsKey(functionName)) instance.rpcReceivedPerFunctionName.Add(functionName, 0);
			instance.rpcReceivedPerFunctionName[functionName]++;
		}
	}
}