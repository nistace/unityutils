using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class TextAssetExtension {
	public static IEnumerable<string> Lines(this TextAsset asset) => asset.text.Split(Environment.NewLine[0]).Select(t => t.Replace("\n", string.Empty));

	public static IReadOnlyDictionary<string, int> CsvHeaderAsDictionary(this TextAsset asset) =>
		SplitCsvLine(asset.text.Split(Environment.NewLine[0])[0]).Select((name, index) => (name, index)).ToDictionary(t => t.name.PascalCase(), t => t.index);

	public static string[] CsvHeaderAsArray(this TextAsset asset) => SplitCsvLine(asset.text.Split(Environment.NewLine[0])[0]);

	public static IEnumerable<string[]> CsvLines(this TextAsset asset) => asset.Lines().Where((t, i) => i > 0 && !string.IsNullOrEmpty(t.Trim())).Select(SplitCsvLine);

	private static string[] SplitCsvLine(string rawLine) {
		var items = new List<string>();
		var charIndex = 0;
		var bufferComa = false;
		var buffer = new StringBuilder();
		while (charIndex < rawLine.Length) {
			if (rawLine[charIndex] == '"') {
				if (rawLine.Length > charIndex + 1 && rawLine[charIndex + 1] == '"') buffer.Append('"');
				bufferComa = !bufferComa;
			}
			else if (!bufferComa && rawLine[charIndex] == ',') {
				items.Add(buffer.ToString());
				buffer.Clear();
			}
			else buffer.Append(rawLine[charIndex]);
			charIndex++;
		}
		if (buffer.Length > 0) items.Add(buffer.ToString());
		return items.ToArray();
	}
}