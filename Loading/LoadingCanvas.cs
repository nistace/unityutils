﻿using System;
using System.Collections;
using NiUtils.Coroutines;
using NiUtils.Extensions;
using NiUtils.Types;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NiUtils.Loading {
	[RequireComponent(typeof(Canvas))]
	public class LoadingCanvas : MonoBehaviour {
		public static LoadingCanvas instance { get; set; }

		[SerializeField] protected Ratio     _initialOpacity = 0f;
		[SerializeField] protected Graphic[] _graphics;
		[SerializeField] protected float     _fadeInSpeed  = 1;
		[SerializeField] protected float     _fadeOutSpeed = 1;
		[SerializeField] protected TMP_Text  _progressText;
		[SerializeField] protected Image     _progressBar;

		private Canvas    sCanvas     { get; set; }
		private Canvas    canvas      => sCanvas ? sCanvas : sCanvas = GetComponent<Canvas>();
		private Ratio     opacity     { get; set; }
		private Coroutine fadeRoutine { get; set; }

		public static UnityEvent onStartFadeIn     { get; } = new UnityEvent();
		public static UnityEvent onFadeInComplete  { get; } = new UnityEvent();
		public static UnityEvent onStartFadeOut    { get; } = new UnityEvent();
		public static UnityEvent onFadeOutComplete { get; } = new UnityEvent();

		private void Start() => SetOpacity(_initialOpacity);
		public void Show(Action callback = null) => fadeRoutine = CoroutineRunner.Run(DoFadeIn(callback));
		public void Hide(Action callback = null) => fadeRoutine = CoroutineRunner.Run(DoFadeOut(callback));

		public IEnumerator DoFadeIn(Action callback = null) {
			onStartFadeIn.Invoke();
			while (opacity < 1) {
				SetOpacity(opacity + Time.deltaTime * _fadeInSpeed);
				yield return null;
			}
			callback?.Invoke();
			onFadeInComplete.Invoke();
		}

		public IEnumerator DoFadeOut(Action callback = null) {
			onStartFadeOut.Invoke();
			while (opacity > 0) {
				SetOpacity(opacity - Time.deltaTime * _fadeOutSpeed);
				yield return null;
			}
			callback?.Invoke();
			onFadeOutComplete.Invoke();
		}

		public void SetVisible() {
			onStartFadeIn.Invoke();
			if (fadeRoutine != null) CoroutineRunner.Stop(fadeRoutine);
			SetOpacity(1);
			onFadeInComplete.Invoke();
		}

		public void SetHidden() {
			onStartFadeOut.Invoke();
			if (fadeRoutine != null) CoroutineRunner.Stop(fadeRoutine);
			SetOpacity(0);
			onFadeOutComplete.Invoke();
		}

		private void SetOpacity(Ratio newOpacity) {
			opacity = newOpacity;
			_graphics.ForEach(t => t.color = t.color.With(a: opacity));
			canvas.enabled = opacity > 0;
		}

		public void SetProgress(float ratio) {
			if (_progressBar) _progressBar.fillAmount = ratio;
			if (_progressText) _progressText.text = $"{Mathf.FloorToInt(ratio * 100):00.00}%";
		}

#if UNITY_EDITOR
		[ContextMenu("Target all children graphics")] private void TargetAllChildrenGraphics() => _graphics = GetComponentsInChildren<Graphic>();
#endif
	}
}