﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils.Coroutines;

namespace Utils.Ui {
	public class ForceRebuildLayoutGroups : MonoBehaviourUi {
		[SerializeField] protected int _frames = 2;

		private SingleCoroutine singleCoroutine { get; set; }
		private LayoutGroup[]   layoutGroups    { get; set; }

		public void Play(int? frames = null) {
			if (singleCoroutine == null) singleCoroutine = new SingleCoroutine(this);
			layoutGroups = GetComponentsInChildren<LayoutGroup>();
			singleCoroutine.Start(DoPlay(frames ?? _frames));
		}

		private IEnumerator DoPlay(int frames) {
			for (var i = 0; i < frames; ++i) {
				foreach (var layoutGroup in layoutGroups) {
					if (layoutGroup) {
						layoutGroup.enabled = false;
						layoutGroup.enabled = true;
					}
				}
				yield return null;
			}
		}
	}
}