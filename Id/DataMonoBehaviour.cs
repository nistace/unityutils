using UnityEngine;

namespace Utils.Id {
	public abstract class DataMonoBehaviour : MonoBehaviour, IData {
		[SerializeField] protected DataId _id;
		public                     int    id => _id.value;

		public bool Equals(IData other) => id == other.id;
	}
}