using System.Collections.Generic;
using UnityEngine;

namespace NiUtils.Libraries {
	public class Particles : MonoBehaviour {
		private static Transform                                 pool           { get; set; }
		private static ParticlesLibrary                          library        { get; set; }
		public static  bool                                      loaded         => library != null;
		private static Dictionary<string, Queue<ParticleSystem>> pooledSystems  { get; } = new Dictionary<string, Queue<ParticleSystem>>();
		private static List<ParticleSystem>                      playingSystems { get; } = new List<ParticleSystem>();

		private void Start() {
			if (pool) return;
			pool = new GameObject("ParticlesPool").transform;
			DontDestroyOnLoad(pool.gameObject);
			pool.SetParent(transform);
			pool.gameObject.SetActive(false);
		}

		public static void LoadLibrary(ParticlesLibrary libraryToLoad) {
			library = libraryToLoad;
			if (library) library.Load();
		}

		public static ParticleSystem Play(string key, Vector3 position, Quaternion? rotation = null, Vector3? scale = null) {
			if (!library) return null;
			var particleSystem = pooledSystems.ContainsKey(key) && pooledSystems[key].Count > 0 ? pooledSystems[key].Dequeue() : library[key] ? Instantiate(library[key]) : null;
			if (!particleSystem) return null;
			particleSystem.name = key;
			var particleSystemTransform = particleSystem.transform;
			particleSystemTransform.SetParent(null);
			particleSystemTransform.position = position;
			particleSystemTransform.rotation = rotation ?? Quaternion.identity;
			particleSystemTransform.localScale = scale ?? Vector3.one;
			particleSystem.Play();
			playingSystems.Add(particleSystem);
			return particleSystem;
		}

		public static void Stop(ParticleSystem system) {
			system.Stop();
			playingSystems.Remove(system);
			if (!pooledSystems.ContainsKey(system.name)) pooledSystems.Add(system.name, new Queue<ParticleSystem>());
			pooledSystems[system.name].Enqueue(system);
			system.transform.SetParent(pool);
		}

		private void Update() {
			if (playingSystems.Count == 0) return;
			for (var i = 0; i < playingSystems.Count; ++i) {
				if (!playingSystems[i]) {
					playingSystems.RemoveAt(i);
					i--;
				}
				else if (!playingSystems[i].isPlaying) Stop(playingSystems[i]);
			}
		}

		public static bool HasKey(string key) => library.HasKey(key);
	}
}