using UnityEngine;
using Utils.Extensions;

namespace Utils.Id {
	public abstract class DataScriptableObject : ScriptableObject, IData {
		[SerializeField] protected DataId _id;
		public                     int    id => _id.value;

		public override string ToString() => name;

		public static E Clone<E>(E origin) where E : DataScriptableObject => Instantiate(origin).Named(origin.name);
	}
}