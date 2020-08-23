using System.Linq;
using MSG;
using UnityEditor;
using UnityEngine;

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
		_text.text = Localisation.Map(key);
	}

	[ContextMenu("Refresh")]
	private void RefreshThis() {
		if (Localisation.loaded) Localisation.Reload();
		else Localisation.SetLanguage(Memory.languages.Values.Single(t => t.defaultLanguage));
		Refresh();
	}

#if UNITY_EDITOR
	[MenuItem("Tools/Localisation/Refresh All L10N Texts")]
	private static void RefreshAll() {
		if (Localisation.loaded) Localisation.Reload();
		else Localisation.SetLanguage(Memory.languages.Values.Single(t => t.defaultLanguage));
		Resources.FindObjectsOfTypeAll<L10NText>().ForEach(t => t.Refresh());
	}
#endif
}