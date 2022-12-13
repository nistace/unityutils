using UnityEngine;
using UnityEditor;

/*
namespace Utils.Types.Editor {
	[CustomPropertyDrawer(typeof(Quaternion))]
	public class QuaternionCustomPropertyDrawer : PropertyDrawer {
		private Vector3 euler       { get; set; }
		private bool    initialized { get; set; }

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			if (!initialized) {
				euler = property.quaternionValue.eulerAngles; // store the current eulerAngles
				initialized = true;
			}

			euler = EditorGUI.Vector3Field(position, label, euler); // display the field.

			property.quaternionValue = Quaternion.Euler(euler); // convert the eulerAngles back to Quaternion value.
		}
	}
}*/