﻿using System;
using TMPro;
using UnityEngine;

public class DebugUi : MonoBehaviour {
	private static DebugUi instance { get; set; }

	[SerializeField] protected TMP_Text  _linePrefab;
	[SerializeField] protected Transform _linesContainer;

	private void Awake() {
		if (instance) Destroy(gameObject);
		else instance = this;
		DontDestroyOnLoad(transform.root.gameObject);
	}

	private void Start() {
		gameObject.SetActive(false);
	}

	public static void Print(string info) {
		var line = Instantiate(instance._linePrefab, instance._linesContainer);
		line.SetText($"[{DateTime.Now:h:mm:ss tt}]{info}");
	}

	public static void Toggle() {
		instance.gameObject.SetActive(!instance.gameObject.activeSelf);
	}
}