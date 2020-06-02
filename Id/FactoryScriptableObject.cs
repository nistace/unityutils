using UnityEngine;

namespace Utils.Id {
	public abstract class FactoryScriptableObject<E> : DataScriptableObject {
		[SerializeField] protected E _prefab;

		public abstract E Instantiate();
	}
}