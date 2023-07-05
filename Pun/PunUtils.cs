#if PHOTON_UNITY_NETWORKING
using Photon.Pun;

namespace NiUtils.Pun {
	public static class PunUtils {
		public static bool offlineOrNoRoom => PhotonNetwork.OfflineMode || PhotonNetwork.CurrentRoom == null;
	}
}
#endif