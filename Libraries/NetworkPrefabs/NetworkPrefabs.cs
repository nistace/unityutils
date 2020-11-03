﻿using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkPrefabs : MonoBehaviour, IPunPrefabPool, IPunOwnershipCallbacks {
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
		Debug.Log($"Instantiating {prefabId} with ownership {photonView.OwnershipTransfer}");
		return photonView.gameObject;
	}

	public void Destroy(GameObject gameObject) {
		if (!gameObject) return;
		Object.Destroy(gameObject);
	}

	public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer) {
		Debug.Log(targetView.name + " requested by " + requestingPlayer.NickName);
	}

	public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner) {
		Debug.Log(targetView.name + " transfered, previous " + previousOwner.NickName);
	}
}