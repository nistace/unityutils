using System.Linq;
using UnityEditor;
using UnityEngine;
using Utils.Extensions;

namespace Utils.Libraries {
	[RequireComponent(typeof(TMPro.TMP_Text))]
	public class L10NText : MonoBehaviour {
		[SerializeField] protected TMPro.TMP_Text _text;
		[SerializeField] protected string         _key;

		public string key {
			get => _key;
			set {
				_key = value;
				Refresh();
			}
		}

		public Color color {
			get => _text.color;
			set => _text.color = value;
		}

		protected void Start() {
			if (string.IsNullOrEmpty(key)) key = _text.text;
			Refresh();
			Localisation.onLanguageChanged.AddListener(Refresh);
		}

		private void Reset() {
			_text = GetComponent<TMPro.TMP_Text>();
			if (string.IsNullOrEmpty(_key)) _key = name.CleanKey();
		}

		private void Refresh() {
			if (!enabled) return;
			if (!_text) _text = GetComponent<TMPro.TMP_Text>();
			if (!_text) return;
			var value = Localisation.Map(key);
			if (_text.text != value) _text.text = Localisation.Map(key);
		}

		[ContextMenu("Refresh")]
		private void RefreshThis() {
			if (Localisation.loaded) Localisation.Reload();
			else Localisation.SetLanguage(Resources.LoadAll<Language>("Localisation").Single(t => t.defaultLanguage));
			Refresh();
		}

#if UNITY_EDITOR
		[MenuItem("Tools/Localisation/Refresh All L10N Texts")]
		private static void RefreshAll() {
			if (Localisation.loaded) Localisation.Reload();
			else Localisation.SetLanguage(Resources.LoadAll<Language>("Localisation").Single(t => t.defaultLanguage));
			Resources.FindObjectsOfTypeAll<L10NText>().ForEach(t => t.Refresh());
		}
#endif
	}
}