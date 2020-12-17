using System;
using Photon.Pun;

namespace Utils.Extensions {
	public static class PunMonoBehaviourPunExtension {
		public static void RpcEncryptedSecureOthers(this MonoBehaviourPun mb, Action func) {
			if (PhotonNetwork.OfflineMode) return;
			mb.photonView.RpcSecure(func.Method.Name, RpcTarget.Others, true);
		}

		public static void RpcEncryptedSecureOthers<E>(this MonoBehaviourPun mb, Action<E> func, E e) {
			if (PhotonNetwork.OfflineMode) return;
			mb.photonView.RpcSecure(func.Method.Name, RpcTarget.Others, true, e);
		}

		public static void RpcEncryptedSecureOthers<E, F>(this MonoBehaviourPun mb, Action<E, F> func, E e, F f) {
			if (PhotonNetwork.OfflineMode) return;
			mb.photonView.RpcSecure(func.Method.Name, RpcTarget.Others, true, e, f);
		}

		public static void RpcOthers<E>(this MonoBehaviourPun mb, Action<E> func, E e) {
			if (PhotonNetwork.OfflineMode) return;
			mb.photonView.RPC(func.Method.Name, RpcTarget.Others, e);
		}

		public static void RpcOthers(this MonoBehaviourPun mb, Action func) {
			if (PhotonNetwork.OfflineMode) return;
			mb.photonView.RPC(func.Method.Name, RpcTarget.Others);
		}
	}
}