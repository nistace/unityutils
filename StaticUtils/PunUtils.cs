using Photon.Pun;

namespace Utils.StaticUtils {
	public static class PunUtils {
		public static bool offlineOrNoRoom => PhotonNetwork.OfflineMode || PhotonNetwork.CurrentRoom == null;
	}
}