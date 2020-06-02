using System;
using System.IO;

public static class FileInfoExtension {
	public static string GetSimpleName(this FileInfo fileInfo) {
		var dotIndex = fileInfo.Name.IndexOf(".", StringComparison.Ordinal);
		if (dotIndex < 0) return fileInfo.Name;
		return fileInfo.Name.Substring(0, dotIndex);
	}
}