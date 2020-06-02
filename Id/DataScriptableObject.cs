using UnityEngine;

public abstract class DataScriptableObject : ScriptableObject, IData {
	[SerializeField] protected DataId _id;
	public                     int    id => _id.value;

	public override string ToString() => name;
}