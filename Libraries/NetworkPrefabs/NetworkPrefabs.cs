using Photon.Pun;
using UnityEngine;

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
		library = libraryToLoad;
		if (library) library.Load();
	}

	public static GameObject Of(string prefabId, Vector3? position = null, Quaternion? rotation = null) =>
		PhotonNetwork.Instantiate(prefabId, position ?? Vector3.zero, rotation ?? Quaternion.identity);

	public static E Of<E>(string prefabId, Vector3? position = null, Quaternion? rotation = null) => Of(prefabId, position, rotation).GetComponent<E>();

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