using Photon.Pun;

namespace Utils.Behaviours {
	public class MonoBehaviourMyPun : MonoBehaviourPun {
		public bool isMine => photonView.IsMine || Equals(photonView.Controller, PhotonNetwork.LocalPlayer);
	}
}