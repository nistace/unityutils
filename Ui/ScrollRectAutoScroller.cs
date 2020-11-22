using UnityEngine;
using UnityEngine.UI;

namespace Utils.Types.Ui {
	public class ScrollRectAutoScroller : MonoBehaviour {
		[SerializeField] protected ScrollRect _scrollRect;
		[SerializeField] protected Scrollbar  _scrollbar;
		[SerializeField] protected int        _duringFrames = 2;

		public  bool  atBottom        => _scrollbar.value < .001f;
		public  bool  atTop           => _scrollbar.value > .999f;
		private int   remainingFrames { get; set; }
		private float target          { get; set; }

		public void ScrollToBottom() => ScrollTo(0, _duringFrames);

		public void ScrollToTop() => ScrollTo(1, _duringFrames);

		private void ScrollTo(float value, int duringFrames) {
			target = value;
			remainingFrames = duringFrames;
		}

		private void LateUpdate() {
			if (remainingFrames <= 0) return;
			_scrollRect.verticalNormalizedPosition = target;
			remainingFrames--;
		}
	}
}