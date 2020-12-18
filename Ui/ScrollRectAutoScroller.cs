using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils.Coroutines;

namespace Utils.Types.Ui {
	public class ScrollRectAutoScroller : MonoBehaviour {
		[SerializeField] protected ScrollRect _scrollRect;
		[SerializeField] protected Scrollbar  _scrollbar;
		[SerializeField] protected int        _duringFrames = 2;

		public  bool            atBottom        => _scrollbar.value < .001f;
		public  bool            atTop           => _scrollbar.value > .999f;
		private SingleCoroutine singleCoroutine { get; set; }

		public void ScrollToBottom() => ScrollTo(0, _duringFrames);

		public void ScrollToTop() => ScrollTo(1, _duringFrames);

		private void ScrollTo(float value, int duringFrames) {
			if (singleCoroutine == null) singleCoroutine = new SingleCoroutine(this);
			singleCoroutine.Start(DoScrollTo(value, duringFrames));
		}

		private IEnumerator DoScrollTo(float value, int duringFrames) {
			for (var i = 0; i < duringFrames; ++i) {
				yield return null;
				_scrollRect.verticalNormalizedPosition = value;
			}
		}
	}
}