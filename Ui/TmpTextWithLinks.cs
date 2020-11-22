using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils.Events;

namespace Utils.Types.Ui {
	[RequireComponent(typeof(TMP_Text))]
	public class TmpTextWithLinks : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
		private static Regex linkRegex { get; } = new Regex("(<link=\".*?\">)(<color=#.*?>)?(.*?)(</color>)?(</link>)");

		[SerializeField] protected Color _linkDefaultColor;
		[SerializeField] protected Color _linkHoverColor;
		[SerializeField] protected Color _linkPressColor;

		private     TMP_Text text             { get; set; }
		private new Camera   camera           { get; set; }
		private     int      hoveredLinkIndex { get; set; } = -1;

		public StringEvent onLinkClicked { get; } = new StringEvent();

		private void Awake() => Init();

		private void Init() {
			if (!text) text = GetComponent<TMP_Text>();
			if (!camera) camera = Camera.current;
		}

		public void SetText(string linksText) {
			Init();
			text.text = linkRegex.Replace(linksText, t => SetLinkRegexMatchColor(t, _linkDefaultColor));
			hoveredLinkIndex = -1;
		}

		private static string SetLinkRegexMatchColor(Match match, Color color) => $"{match.Groups[1]}<color={color.ToHexaString()}>{match.Groups[3]}</color>{match.Groups[5]}";

		private void SetLinkColor(int linkIndex, Color color) {
			var matches = linkRegex.Matches(text.text);
			if (matches.Count <= linkIndex) return;

			var transformedString = new StringBuilder();
			transformedString.Append(text.text.Substring(0, matches[linkIndex].Index));
			transformedString.Append(SetLinkRegexMatchColor(matches[linkIndex], color));
			transformedString.Append(text.text.Substring(matches[linkIndex].Index + matches[linkIndex].Length));

			text.text = transformedString.ToString();
		}

		private void LateUpdate() {
			var newLinkIndex = TMP_TextUtilities.IsIntersectingRectTransform(text.rectTransform, Input.mousePosition, camera)
				? TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, camera)
				: -1;

			if (newLinkIndex == hoveredLinkIndex) return;
			if (hoveredLinkIndex >= 0) SetLinkColor(hoveredLinkIndex, _linkDefaultColor);
			hoveredLinkIndex = newLinkIndex;
			if (hoveredLinkIndex >= 0) SetLinkColor(hoveredLinkIndex, _linkHoverColor);
		}

		public void OnPointerDown(PointerEventData eventData) {
			if (hoveredLinkIndex < 0) return;
			SetLinkColor(hoveredLinkIndex, _linkPressColor);
		}

		public void OnPointerUp(PointerEventData eventData) {
			if (hoveredLinkIndex < 0) return;
			SetLinkColor(hoveredLinkIndex, _linkHoverColor);
			onLinkClicked.Invoke(text.textInfo.linkInfo[hoveredLinkIndex].GetLinkID());
		}
	}
}