using UnityEditor;
using UnityEngine;

namespace Utils.Ui.Editor {
	[CustomPropertyDrawer(typeof(RectTransformPosition))]
	public class RectTransformPositionPropertyDrawer : PropertyDrawer {
		private SerializedProperty activeProperty { get; set; }

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => property.isExpanded ? EditorGUIUtility.singleLineHeight * 4 : EditorGUIUtility.singleLineHeight;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && position.Contains(Event.current.mousePosition)) {
				property.isExpanded = !property.isExpanded;
			}

			if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && position.Contains(Event.current.mousePosition)) {
				activeProperty = property;
				var context = new GenericMenu();
				context.AddItem(new GUIContent("Save current values"), false, SaveCurrentTransformValues);
				context.AddItem(new GUIContent("Apply to Transform"), false, ApplyToTransform);
				context.ShowAsContext();
			}

			EditorGUI.BeginProperty(position, label, property);

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			if (property.isExpanded) {
				const int firstLabelWidth = 50;
				const int secondLabelWidth = 30;
				const int labelsWidth = firstLabelWidth + secondLabelWidth;
				var remainingWidth = position.width - (firstLabelWidth + secondLabelWidth);

				var h = EditorGUIUtility.singleLineHeight;
				EditorGUI.LabelField(new Rect(position.x, position.y, firstLabelWidth, h), "Anchor", new GUIStyle { alignment = TextAnchor.MiddleLeft });
				EditorGUI.LabelField(new Rect(position.x + firstLabelWidth, position.y, secondLabelWidth, h), "Min", new GUIStyle { alignment = TextAnchor.MiddleLeft });
				EditorGUI.LabelField(new Rect(position.x + firstLabelWidth, position.y + h, secondLabelWidth, h), "Max", new GUIStyle { alignment = TextAnchor.MiddleLeft });
				EditorGUI.LabelField(new Rect(position.x, position.y + 2 * h, firstLabelWidth, h), "Offset", new GUIStyle { alignment = TextAnchor.MiddleLeft });
				EditorGUI.LabelField(new Rect(position.x + firstLabelWidth, position.y + 2 * h, secondLabelWidth, h), "Min", new GUIStyle { alignment = TextAnchor.MiddleLeft });
				EditorGUI.LabelField(new Rect(position.x + firstLabelWidth, position.y + 3 * h, secondLabelWidth, h), "Max", new GUIStyle { alignment = TextAnchor.MiddleLeft });

				EditorGUI.PropertyField(new Rect(position.x + labelsWidth, position.y, remainingWidth, position.height), property.FindPropertyRelative("_anchorMin"), GUIContent.none);
				EditorGUI.PropertyField(new Rect(position.x + labelsWidth, position.y + h, remainingWidth, position.height), property.FindPropertyRelative("_anchorMax"), GUIContent.none);
				EditorGUI.PropertyField(new Rect(position.x + labelsWidth, position.y + 2 * h, remainingWidth, position.height), property.FindPropertyRelative("_offsetMin"), GUIContent.none);
				EditorGUI.PropertyField(new Rect(position.x + labelsWidth, position.y + 3 * h, remainingWidth, position.height), property.FindPropertyRelative("_offsetMax"), GUIContent.none);
			}
			else {
				EditorGUI.LabelField(position, "<Click for detail>", new GUIStyle { alignment = TextAnchor.MiddleLeft });
			}

			EditorGUI.indentLevel = indent;
			EditorGUI.EndProperty();
		}

		private void ApplyToTransform() {
			var transform = ((Behaviour)activeProperty.serializedObject.targetObject).GetComponent<RectTransform>();
			transform.anchorMin = activeProperty.FindPropertyRelative("_anchorMin").vector2Value;
			transform.anchorMax = activeProperty.FindPropertyRelative("_anchorMax").vector2Value;
			transform.offsetMin = activeProperty.FindPropertyRelative("_offsetMin").vector2Value;
			transform.offsetMax = activeProperty.FindPropertyRelative("_offsetMax").vector2Value;
		}

		private void SaveCurrentTransformValues() {
			activeProperty.serializedObject.Update();
			var transform = ((Behaviour)activeProperty.serializedObject.targetObject).GetComponent<RectTransform>();
			activeProperty.FindPropertyRelative("_anchorMin").vector2Value = transform.anchorMin;
			activeProperty.FindPropertyRelative("_anchorMax").vector2Value = transform.anchorMax;
			activeProperty.FindPropertyRelative("_offsetMin").vector2Value = transform.offsetMin;
			activeProperty.FindPropertyRelative("_offsetMax").vector2Value = transform.offsetMax;
			activeProperty.serializedObject.ApplyModifiedProperties();
		}
	}
}