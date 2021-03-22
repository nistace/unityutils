using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Utils.Extensions;
using Utils.Id;

namespace Utils.Libraries {
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
						var previousLineKey = string.Empty;
						foreach (var line in blockLines.OrderBy(t => t)) {
							if (line.IndexOf("=", StringComparison.Ordinal) < 0) continue;
							var key = line.Substring(0, line.IndexOf("=", StringComparison.Ordinal)).CleanKey();
							if (previousLineKey == key) {
								Debug.Log("Duplicate entry for key " + key);
							}
							previousLineKey = key;
							writer.WriteLine(line);
						}
						writer.WriteLine(string.Empty);
					}
				}
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

				var buffer = new StringBuilder();

				foreach (var line in lines) {
					if (line[keyColumnIndex].StartsWith("#")) buffer.Append(Environment.NewLine).Append(line[keyColumnIndex]).Append(Environment.NewLine);
					else if (!string.IsNullOrEmpty(line[keyColumnIndex]) && line.Length > langColumnIndex && !string.IsNullOrEmpty(line[langColumnIndex]))
						buffer.Append(line[keyColumnIndex].CleanKey(true)).Append(" = ").Append(line[langColumnIndex]).Append(Environment.NewLine);
				}
				File.WriteAllText(AssetDatabase.GetAssetPath(langTextAsset), buffer.ToString());
				EditorUtility.SetDirty(langTextAsset);
				Debug.Log($"{lang.CamelCase()}.properties updated.");
			}
			AssetDatabase.SaveAssets();
			SortLinesInFiles();
		}
#endif
	}
}