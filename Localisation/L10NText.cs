using System.Collections.Generic;
using System.IO;
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
		if (!_text) _text = GetComponent<TMPro.TMP_Text>();
		if (!_text) return;
		_text.text = Localisation.Map(key);
	}

#if UNITY_EDITOR

	[ContextMenu("Refresh")]
	private void RefreshThis() {
		if (Localisation.loaded) Localisation.Reload();
		else Localisation.SetLanguage(Memory.languages.Values.Single(t => t.defaultLanguage));
		Refresh();
	}

	[MenuItem("Tools/Localisation/Refresh All L10N Texts")]
	private static void RefreshAll() {
		if (Localisation.loaded) Localisation.Reload();
		else Localisation.SetLanguage(Memory.languages.Values.Single(t => t.defaultLanguage));
		Resources.FindObjectsOfTypeAll<L10NText>().ForEach(t => t.Refresh());
	}

	[MenuItem("Tools/Localisation/Sort Keys In Files")]
	public static void SortLinesInFiles() {
		foreach (var textAsset in Resources.LoadAll<TextAsset>("Localisation")) {
			var path = AssetDatabase.GetAssetPath(textAsset);
			var allLines = textAsset.Lines().Select(t => t.Replace("\n", string.Empty).Trim()).Where(t => !string.IsNullOrEmpty(t)).ToList();

			using (var writer = new StreamWriter(path, false)) {
				while (allLines.Count > 0) {
					while (allLines.Count > 0 && allLines[0].StartsWith("#")) {
						writer.WriteLine(allLines[0]);
						allLines.RemoveAt(0);
					}
					var blockLines = new List<string>();
					while (allLines.Count > 0 && !allLines[0].StartsWith("#")) {
						blockLines.Add(allLines[0]);
						allLines.RemoveAt(0);
					}
					foreach (var line in blockLines.OrderBy(t => t)) writer.WriteLine(line);
					writer.WriteLine(string.Empty);
				}
			}
			AssetDatabase.ImportAsset(path);
			EditorUtility.SetDirty(textAsset);
		}
		AssetDatabase.SaveAssets();
	}
#endif
}