using ExitGames.Client.Photon;

namespace Utils.Saving {
	public interface ISaveToHashtable {
		void Save(Hashtable data, string root);
	}
}