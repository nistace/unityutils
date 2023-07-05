#if PHOTON_UNITY_NETWORKING
using ExitGames.Client.Photon;

namespace NiUtils.Pun.Saving {
	public interface ISaveToHashtable {
		void Save(Hashtable data, string root);
	}
}
#endif