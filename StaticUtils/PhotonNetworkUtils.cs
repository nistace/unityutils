using Photon.Pun;
using UnityEngine;

namespace Utils.StaticUtils {
	public static class PhotonNetworkUtils {
		public static E Instantiate<E>(E prefab) where E : MonoBehaviour => Instantiate(prefab, Vector3.zero, Quaternion.identity);

		public static E Instantiate<E>(E prefab, Vector3 position, Quaternion rotation) where E : MonoBehaviour => PhotonNetwork.Instantiate(prefab.name, position, rotation).GetComponent<E>();
	}
}