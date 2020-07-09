using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TextAssetExtension {
	public static IEnumerable<string> Lines(this TextAsset asset) => asset.text.Split(Environment.NewLine[0]).Select(t => t.Replace("\n", string.Empty));

	public static IReadOnlyDictionary<string, int> CsvHeader(this TextAsset asset) =>
		asset.Lines().First().Split(',').Select((name, index) => (name, index)).ToDictionary(t => t.name.PascalCase(), t => t.index);

	public static IEnumerable<string[]> CsvLines(this TextAsset asset) => asset.Lines().Where((t, i) => i > 0 && !string.IsNullOrEmpty(t.Trim())).Select(t => t.Split(','));
}