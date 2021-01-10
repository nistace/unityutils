using Photon.Pun;
using UnityEngine;

namespace Utils.Multi {
	[RequireComponent(typeof(PhotonView))]
	public class MonoBehaviourMyPun : MonoBehaviourPun {
		public bool isMine => photonView.IsMine || Equals(photonView.Controller, PhotonNetwork.LocalPlayer);
	}
}