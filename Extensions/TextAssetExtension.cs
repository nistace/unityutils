using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TextAssetExtension {
	public static IEnumerable<string> Lines(this TextAsset asset) => asset.text.Split(Environment.NewLine[0]).Select(t => t.Replace("\n", string.Empty));
}