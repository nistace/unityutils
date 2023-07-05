using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NiUtils.Extensions;
using NiUtils.Id;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace NiUtils.Libraries {
	[CreateAssetMenu(menuName = "Constants/Language")]
	public class Language : DataScriptableObject {
		public class Event : UnityEvent<Language> { }

		[SerializeField] protected string           _iso;
		[SerializeField] protected TextAsset        _textAsset;
		[SerializeField] protected bool             _alwaysLoad;
		[SerializeField] protected bool             _defaultLanguage;
		[SerializeField] protected SystemLanguage[] _languages;
		[SerializeField] protected int              _index;
		[SerializeField] protected bool             _active = true;

		public TextAsset                   textAsset       => _textAsset;
		public bool                        alwaysLoad      => _alwaysLoad;
		public string                      iso             => _iso;
		public IEnumerable<SystemLanguage> languages       => _languages;
		public bool                        defaultLanguage => _defaultLanguage;
		public int                         index           => _index;
		public bool                        active          => _active;

#if UNITY_EDITOR

		[MenuItem("Tools/Localisation/Sort Keys In Files")]
		public static void SortLinesInFiles() {
			foreach (var textAsset in Resources.LoadAll<TextAsset>("Localisation")) {
				if (textAsset.name.In("allTexts", "how-to")) continue;
				var path = AssetDatabase.GetAssetPath(textAsset);
				WriteSortedLines(path, textAsset.Lines().Select(t => t.Replace("\n", string.Empty).Trim()).Where(t => !string.IsNullOrEmpty(t)).ToList());
				AssetDatabase.ImportAsset(path);
				EditorUtility.SetDirty(textAsset);
				Debug.Log($"Keys sorted in {textAsset.name} asset file.");
			}
			AssetDatabase.SaveAssets();
		}

		[MenuItem("Tools/Localisation/Import CSV ")]
		public static void ImportCsv() {
			Debug.ClearDeveloperConsole();
			var textAsset = Resources.Load<TextAsset>("Sheets/localisation");
			if (!textAsset) {
				Debug.LogWarning("No csv file at Sheets/localisation");
				return;
			}
			var columns = textAsset.CsvHeaderAsDictionary();
			var keyColumnIndex = columns["Key"];
			var lines = textAsset.CsvLines().ToArray();
			foreach (var lang in columns.Keys) {
				if (lang.Trim().Length == 0) continue;
				if (lang.ToLower().In("key", "parameters")) continue;
				var langColumnIndex = columns[lang];
				var langTextAsset = Resources.Load<TextAsset>($"Localisation/{lang.CamelCase()}");
				if (!langTextAsset) {
					AssetDatabase.CreateAsset(new TextAsset(), $"Assets/Resources/Localisation/{lang.CamelCase()}.txt");
					AssetDatabase.SaveAssets();
					langTextAsset = Resources.Load<TextAsset>($"Localisation/{lang.CamelCase()}");
					Debug.Log($"Assets/Resources/Localisation/{lang.CamelCase()}.properties created.");
				}
				var path = AssetDatabase.GetAssetPath(langTextAsset);

				var langLines = lines.Select(t => (key: t.GetSafe(keyColumnIndex, string.Empty).Trim(), value: t.GetSafe(langColumnIndex, string.Empty).Trim()))
					.Where(t => !string.IsNullOrEmpty(t.key) && (t.key.StartsWith("#") || !string.IsNullOrEmpty(t.value))).Select(t => t.key.StartsWith("#") ? t.key : $"{t.key} = {t.value}");

				WriteSortedLines(path, langLines);
				AssetDatabase.ImportAsset(path);
				EditorUtility.SetDirty(langTextAsset);
				Debug.Log($"{lang.CamelCase()}.properties updated.");
			}
			AssetDatabase.SaveAssets();
		}

		private static void WriteSortedLines(string path, IEnumerable<string> langLines) {
			var linesAsList = langLines.ToList();
			using (var writer = new StreamWriter(path, false)) {
				while (linesAsList.Count > 0) {
					while (linesAsList.Count > 0 && linesAsList[0].StartsWith("#")) {
						writer.WriteLine(linesAsList[0]);
						linesAsList.RemoveAt(0);
					}
					var blockLines = new List<string>();
					while (linesAsList.Count > 0 && !linesAsList[0].StartsWith("#")) {
						blockLines.Add(linesAsList[0]);
						linesAsList.RemoveAt(0);
					}
					var previousLineKey = string.Empty;
					foreach (var line in blockLines.OrderBy(t => t)) {
						var key = line.Substring(0, line.IndexOf("=", StringComparison.Ordinal)).CleanKey();
						if (previousLineKey == key) Debug.Log("Duplicate entry for key " + key);
						previousLineKey = key;
						writer.WriteLine(line);
					}
					writer.WriteLine(string.Empty);
				}
			}
		}

#endif
	}
}