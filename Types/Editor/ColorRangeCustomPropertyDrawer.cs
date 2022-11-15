using UnityEditor;
using UnityEngine;

namespace Utils.Types.Editor {
	[CustomPropertyDrawer(typeof(ColorRange))]
	public class ColorRangeCustomPropertyDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			var w3 = position.width / 3;

			EditorGUI.PropertyField(new Rect(position.x + 0 * w3, position.y, w3, position.height), property.FindPropertyRelative("_first"), GUIContent.none);
			EditorGUI.LabelField(new Rect(position.x + 1 * w3, position.y, w3, position.height), "=>", new GUIStyle { alignment = TextAnchor.MiddleCenter });
			EditorGUI.PropertyField(new Rect(position.x + 2 * w3, position.y, w3, position.height), property.FindPropertyRelative("_second"), GUIContent.none);

			EditorGUI.indentLevel = indent;
			EditorGUI.EndProperty();
		}
	}
}