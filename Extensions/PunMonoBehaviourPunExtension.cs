using System;
using Photon.Pun;
using Photon.Realtime;
using Utils.Multi.Profiling;
using Utils.StaticUtils;

namespace Utils.Extensions {
	public static class PunMonoBehaviourPunExtension {
		public static void RpcSecure(this MonoBehaviourPun mb, RpcTarget target, Action func) {
			if (PunUtils.offlineOrNoRoom) {
				if (target == RpcTarget.All) func.Invoke();
				return;
			}
			mb.photonView.RpcSecure(func.Method.Name, target, true);
			PunRpcProfiler.AddRpcSent(func.Method.Name);
		}

		public static void RpcSecure<E>(this MonoBehaviourPun mb, RpcTarget target, Action<E> func, E e) {
			if (PunUtils.offlineOrNoRoom) {
				if (target == RpcTarget.All) func.Invoke(e);
				return;
			}
			mb.photonView.RpcSecure(func.Method.Name, target, true, e);
			PunRpcProfiler.AddRpcSent(func.Method.Name);
		}

		public static void RpcSecure(this MonoBehaviourPun mb, Player player, Action func) {
			if (PunUtils.offlineOrNoRoom) {
				if (Equals(player, PhotonNetwork.LocalPlayer)) func.Invoke();
				return;
			}
			mb.photonView.RpcSecure(func.Method.Name, player, true);
			PunRpcProfiler.AddRpcSent(func.Method.Name);
		}

		public static void RpcSecure<E>(this MonoBehaviourPun mb, Player player, Action<E> func, E e) {
			if (PunUtils.offlineOrNoRoom) {
				if (Equals(player, PhotonNetwork.LocalPlayer)) func.Invoke(e);
				return;
			}
			mb.photonView.RpcSecure(func.Method.Name, player, true, e);
			PunRpcProfiler.AddRpcSent(func.Method.Name);
		}

		public static void Rpc(this MonoBehaviourPun mb, RpcTarget target, Action func) {
			if (PunUtils.offlineOrNoRoom) {
				if (target == RpcTarget.All) func.Invoke();
				return;
			}
			mb.photonView.RPC(func.Method.Name, target);
			PunRpcProfiler.AddRpcSent(func.Method.Name);
		}

		public static void Rpc<E>(this MonoBehaviourPun mb, RpcTarget target, Action<E> func, E e) {
			if (PunUtils.offlineOrNoRoom) {
				if (target == RpcTarget.All) func.Invoke(e);
				return;
			}
			mb.photonView.RPC(func.Method.Name, target, e);
			PunRpcProfiler.AddRpcSent(func.Method.Name);
		}

		public static void Rpc(this MonoBehaviourPun mb, Player player, Action func) {
			if (PunUtils.offlineOrNoRoom) {
				if (Equals(player, PhotonNetwork.LocalPlayer)) func.Invoke();
				return;
			}
			mb.photonView.RPC(func.Method.Name, player);
			PunRpcProfiler.AddRpcSent(func.Method.Name);
		}

		public static void Rpc<E>(this MonoBehaviourPun mb, Player player, Action<E> func, E e) {
			if (PunUtils.offlineOrNoRoom) {
				if (Equals(player, PhotonNetwork.LocalPlayer)) func.Invoke(e);
				return;
			}
			mb.photonView.RPC(func.Method.Name, player, e);
			PunRpcProfiler.AddRpcSent(func.Method.Name);
		}

		public static void RpcMaster(this MonoBehaviourPun mb, Action func) {
			if (PunUtils.offlineOrNoRoom) {
				func.Invoke();
				return;
			}
			mb.photonView.RPC(func.Method.Name, RpcTarget.MasterClient);
			PunRpcProfiler.AddRpcSent(func.Method.Name);
		}

		public static void RpcMaster<E>(this MonoBehaviourPun mb, Action<E> func, E e) {
			if (PunUtils.offlineOrNoRoom) {
				func.Invoke(e);
				return;
			}
			mb.photonView.RPC(func.Method.Name, RpcTarget.MasterClient, e);
			PunRpcProfiler.AddRpcSent(func.Method.Name);
		}
	}
}