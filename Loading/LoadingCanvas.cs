using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Coroutines;
using Utils.Extensions;
using Utils.Types;

namespace Utils.Loading {
	[RequireComponent(typeof(Canvas))]
	public class LoadingCanvas : MonoBehaviour {
		[SerializeField] protected Ratio     _initialOpacity = 0f;
		[SerializeField] protected Graphic[] _graphics;
		[SerializeField] protected float     _fadeInSpeed  = 1;
		[SerializeField] protected float     _fadeOutSpeed = 1;

		private Canvas          sCanvas       { get; set; }
		private Canvas          canvas        => sCanvas ? sCanvas : sCanvas = GetComponent<Canvas>();
		private Ratio           opacity       { get; set; }
		private SingleCoroutine fadeCoroutine { get; set; }

		public static UnityEvent onStartFadeIn     { get; } = new UnityEvent();
		public static UnityEvent onFadeInComplete  { get; } = new UnityEvent();
		public static UnityEvent onStartFadeOut    { get; } = new UnityEvent();
		public static UnityEvent onFadeOutComplete { get; } = new UnityEvent();

		private void Awake() => fadeCoroutine = new SingleCoroutine(this);
		private void Start() => SetOpacity(_initialOpacity);
		public void Show() => fadeCoroutine.Start(DoFadeIn());
		public void Hide() => fadeCoroutine.Start(DoFadeOut());

		private IEnumerator DoFadeIn() {
			onStartFadeIn.Invoke();
			while (opacity < 1) {
				SetOpacity(opacity + Time.deltaTime / _fadeInSpeed);
				yield return null;
			}
			onFadeInComplete.Invoke();
		}

		private IEnumerator DoFadeOut() {
			onStartFadeOut.Invoke();
			while (opacity > 0) {
				SetOpacity(opacity - Time.deltaTime / _fadeOutSpeed);
				yield return null;
			}
			onFadeOutComplete.Invoke();
		}

		public void SetVisible() {
			onStartFadeIn.Invoke();
			fadeCoroutine.Stop();
			SetOpacity(1);
			onFadeInComplete.Invoke();
		}

		public void SetHidden() {
			onStartFadeOut.Invoke();
			fadeCoroutine.Stop();
			SetOpacity(0);
			onFadeOutComplete.Invoke();
		}

		private void SetOpacity(Ratio newOpacity) {
			opacity = newOpacity;
			_graphics.ForEach(t => t.color = t.color.With(a: opacity));
			canvas.enabled = opacity > 0;
		}

#if UNITY_EDITOR
		[ContextMenu("Target all children graphics")] private void TargetAllChildrenGraphics() => _graphics = GetComponentsInChildren<Graphic>();
#endif
	}
}