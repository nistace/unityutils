using System.Collections.Generic;
using NiUtils.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace NiUtils.Ui {
	public class HorizontalSeparatedBarUi : MonoBehaviourUi {
		[SerializeField] protected int         _parts;
		[SerializeField] protected Color       _separatorColor;
		[SerializeField] protected float       _separatorWidth;
		[SerializeField] protected List<Image> _separators = new List<Image>();

		public void Build(int parts) {
			_parts = parts;
			Build();
		}

		[ContextMenu("Build")]
		public void Build() {
			while (_separators.Count < _parts - 1) {
				var newSeparator = new GameObject("Separator (Auto generated)").ParentedTo(transform).AddComponent<Image>();
				newSeparator.color = _separatorColor;
				_separators.Add(newSeparator);
			}

			for (var i = 0; i < _parts - 1; ++i) {
				_separators[i].gameObject.SetActive(true);
				_separators[i].rectTransform.anchorMin = new Vector2((float)(i + 1) / _parts, 0);
				_separators[i].rectTransform.anchorMax = new Vector2((float)(i + 1) / _parts, 1);
				_separators[i].rectTransform.offsetMin = new Vector2(-_separatorWidth / 2f, 0);
				_separators[i].rectTransform.offsetMax = new Vector2(_separatorWidth / 2f, 0);
			}

			for (var i = _parts - 1; i < _separators.Count; ++i) _separators[i].gameObject.SetActive(false);
		}
	}
}