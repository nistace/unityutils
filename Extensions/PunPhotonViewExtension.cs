﻿using System;
using Photon.Pun;

namespace Utils.Extensions {
	public static class PunPhotonViewExtension {
		public static void Rpc(this PhotonView photonView, Action func, RpcTarget target) => photonView.RPC(nameof(func), target);
		public static void Rpc<E>(this PhotonView photonView, Action<E> func, RpcTarget target, E param1) => photonView.RPC(nameof(func), target, param1);
		public static void Rpc<E, F>(this PhotonView photonView, Action<E, F> func, RpcTarget target, E param1, F param2) => photonView.RPC(nameof(func), target, param1, param2);
		public static void Rpc<E, F, G>(this PhotonView photonView, Action<E, F, G> func, RpcTarget target, E param1, F param2, G param3) => photonView.RPC(nameof(func), target, param1, param2, param3);

		public static void Rpc<E, F, G, H>(this PhotonView photonView, Action<E, F, G, H> func, RpcTarget target, E param1, F param2, G param3, H param4) =>
			photonView.RPC(nameof(func), target, param1, param2, param3, param4);

		public static void Rpc<E, F, G, H, I>(this PhotonView photonView, Action<E, F, G, H, I> func, RpcTarget target, E param1, F param2, G param3, H param4, I param5) =>
			photonView.RPC(nameof(func), target, param1, param2, param3, param4, param5);
	}
}