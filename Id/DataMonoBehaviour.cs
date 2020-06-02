﻿using UnityEngine;

public abstract class DataMonoBehaviour : MonoBehaviour, IData {
	[SerializeField] protected DataId _id;
	public                     int    id => _id.value;
}