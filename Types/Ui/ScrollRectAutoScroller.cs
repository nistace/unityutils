using UnityEngine;
using UnityEngine.UI;

namespace Utils.Types.Ui {
	public class ScrollRectAutoScroller : MonoBehaviour {
		[SerializeField] protected ScrollRect _scrollRect;
		[SerializeField] protected Scrollbar  _scrollbar;
		[SerializeField] protected int        _duringFrames = 2;

		public  bool  atBottom => _scrollbar.value < .001f;
		public  bool  atTop    => _scrollbar.value > .999f;
		private float target   { get; set; }

		public void ScrollToBottom(int? duringFrames = null) => ScrollTo(0, duringFrames ?? _duringFrames);

		public void ScrollToTop(int? duringFrames = null) => ScrollTo(1, duringFrames ?? _duringFrames);

		private void ScrollTo(float value, int duringFrames) {
			target = value;
			_duringFrames = duringFrames;
		}

		private void LateUpdate() {
			if (_duringFrames <= 0) return;
			_scrollRect.verticalNormalizedPosition = target;
			_duringFrames--;
		}
	}
}