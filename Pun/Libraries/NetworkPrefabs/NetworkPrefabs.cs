using NiUtils.Extensions;
using Photon.Pun;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NiUtils.Pun.Libraries.Network {
	public class NetworkPrefabs : MonoBehaviour, IPunPrefabPool {
		private static NetworkPrefabs        instance { get; set; }
		private static NetworkPrefabsLibrary library  { get; set; }
		public static  bool                  loaded   => library != null;

		private void Awake() {
			if (instance) Destroy(gameObject);
			else {
				instance = this;
				PhotonNetwork.PrefabPool = this;
				PhotonNetwork.AddCallbackTarget(this);
				if (transform != transform.root) transform.SetParent(null);
				DontDestroyOnLoad(gameObject);
			}
		}

		public static void LoadLibrary(NetworkPrefabsLibrary libraryToLoad) {
			if (!instance) {
				var newInstance = new GameObject("NetworkPrefabs");
				newInstance.AddComponent<NetworkPrefabs>();
				DontDestroyOnLoad(newInstance);
			}
			library = libraryToLoad;
			if (library) library.Load();
		}

		private static GameObject Of(string prefabId, Vector3? position = null, Quaternion? rotation = null, bool persistent = false) {
			if (PunUtils.offlineOrNoRoom) return instance.Instantiate(prefabId, position ?? Vector3.zero, rotation ?? Quaternion.identity).Active();
			if (persistent) return PhotonNetwork.InstantiateRoomObject(prefabId, position ?? Vector3.zero, rotation ?? Quaternion.identity);
			return PhotonNetwork.Instantiate(prefabId, position ?? Vector3.zero, rotation ?? Quaternion.identity);
		}

		public static E Of<E>(string prefabId, Vector3? position = null, Quaternion? rotation = null, bool persistent = false) => Of(prefabId, position, rotation, persistent).GetComponent<E>();
		public static E Of<E>(Vector3? position = null, Quaternion? rotation = null, bool persistent = false) => Of(typeof(E).Name, position, rotation, persistent).GetComponent<E>();

		public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation) {
			if (!library) return null;
			if (!library[prefabId]) return null;
			var photonView = Object.Instantiate(library[prefabId], position, rotation).Inactive().GetComponent<PhotonView>();
			return photonView.gameObject;
		}

		public void Destroy(GameObject gameObject) {
			if (!gameObject) return;
			Object.Destroy(gameObject);
		}
	}
}