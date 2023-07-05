using UnityEditor;
using UnityEngine;

namespace NiUtils.Id.Editor {
	[CustomPropertyDrawer(typeof(DataId))]
	public class DataIdCustomPropertyDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			EditorGUI.PropertyField(new Rect(position.x, position.y, 80, position.height), property.FindPropertyRelative("_value"), GUIContent.none);

			if (property.FindPropertyRelative("_value").intValue == 0 || GUI.Button(new Rect(position.x + 80, position.y, position.width - 80, position.height), "Next")) {
				property.FindPropertyRelative("_value").intValue = DataId.GetNextId();
			}

			EditorGUI.indentLevel = indent;
			EditorGUI.EndProperty();
		}
	}
}