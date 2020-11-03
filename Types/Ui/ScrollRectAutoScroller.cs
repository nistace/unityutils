using UnityEngine;
using UnityEngine.UI;

namespace Utils.Types.Ui {
	public class ScrollRectAutoScroller : MonoBehaviour {
		[SerializeField] protected ScrollRect _scrollRect;
		[SerializeField] protected Scrollbar  _scrollbar;
		[SerializeField] protected int        _duringFrames;
		[SerializeField] protected float      _target;

		public bool atBottom => _scrollbar.value < .001f;
		public bool atTop    => _scrollbar.value > .999f;

		public void ScrollToBottom(int duringFrames = 2) => ScrollTo(0, duringFrames);

		public void ScrollToTop(int duringFrames = 2) => ScrollTo(1, duringFrames);

		private void ScrollTo(float value, int duringFrames) {
			_target = value;
			_duringFrames = duringFrames;
		}

		private void LateUpdate() {
			if (_duringFrames <= 0) return;
			_scrollRect.verticalNormalizedPosition = _target;
			_duringFrames--;
		}
	}
}