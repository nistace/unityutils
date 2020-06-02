using System;
using System.Collections.Generic;
using System.Xml;

public static class XmlDocumentExtension {
	public static XmlElement Element(this XmlNode node, string name) {
		if (!(node is XmlDocument) && node.OwnerDocument == null) throw new NullReferenceException();
		var element = (node as XmlDocument ?? node.OwnerDocument).CreateElement(name);
		node.AppendChild(element);
		return element;
	}

	public static XmlElement WithAttributes(this XmlElement node, params (string name, object value)[] attributes) {
		foreach ((var name, var value) in attributes) node.SetAttribute(name, $"{value}");
		return node;
	}

	public static bool TryIntAttribute(this XmlNode node, string attributeName, out int value) {
		value = default;
		if (node.Attributes[attributeName]?.Value == null) return false;
		return int.TryParse(node.Attributes[attributeName].Value, out value);
	}

	public static int IntAttribute(this XmlNode node, string attributeName, int defaultValue = 0) => node.TryIntAttribute(attributeName, out var value) ? value : defaultValue;

	public static float FloatAttribute(this XmlNode element, string attributeName, float defaultValue = 0) =>
		float.TryParse(element?.Attributes[attributeName]?.Value ?? "", out var value) ? value : defaultValue;

	public static string StringAttribute(this XmlNode element, string attributeName, string defaultValue = "") => element?.Attributes[attributeName]?.Value ?? defaultValue;

	public static bool BoolAttribute(this XmlNode element, string attributeName, bool defaultValue = false) =>
		bool.TryParse(element?.Attributes[attributeName]?.Value ?? "", out var value) ? value : defaultValue;

	public static IEnumerable<XmlNode> Nodes(this XmlNode parent, string name) {
		var result = new List<XmlNode>();
		var nodes = parent.SelectNodes(name);
		for (var i = 0; i < nodes.Count; ++i) result.Add(nodes[i]);
		return result;
	}
}