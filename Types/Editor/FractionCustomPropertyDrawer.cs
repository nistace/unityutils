using UnityEditor;
using UnityEngine;

namespace Utils.Types.Editor {
	[CustomPropertyDrawer(typeof(Fraction))]
	public class FractionCustomPropertyDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			var w8 = position.width / 8;

			EditorGUI.PropertyField(new Rect(position.x, position.y, 2 * w8, position.height), property.FindPropertyRelative("_numerator"), GUIContent.none);
			EditorGUI.LabelField(new Rect(position.x + 2 * w8, position.y, w8, position.height), "/", new GUIStyle { alignment = TextAnchor.MiddleCenter });
			EditorGUI.PropertyField(new Rect(position.x + 3 * w8, position.y, 2 * w8, position.height), property.FindPropertyRelative("_denominator"), GUIContent.none);
			EditorGUI.LabelField(new Rect(position.x + 5 * w8, position.y, 3 * w8, position.height),
				$" = {(float)property.FindPropertyRelative("_numerator").intValue / property.FindPropertyRelative("_denominator").intValue:0.00}", new GUIStyle { alignment = TextAnchor.MiddleCenter });

			EditorGUI.indentLevel = indent;
			EditorGUI.EndProperty();
		}
	}
}