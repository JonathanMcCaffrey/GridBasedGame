using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(ButtonDefault))]
public class ButtonDefaultDrawer : PropertyDrawer {
	SerializedProperty disableModifiedControls;
	SerializedProperty background;
	SerializedProperty button;
	SerializedProperty backgroundColor;
	SerializedProperty onButtonClick;
	
	bool notReady() {
		return !background.objectReferenceValue || !button.objectReferenceValue;
	}

	public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label) {
		disableModifiedControls = prop.FindPropertyRelative ("disableModifiedControls");
		background = prop.FindPropertyRelative ("background");
		button = prop.FindPropertyRelative ("button");
		backgroundColor = prop.FindPropertyRelative ("backgroundColor");
		onButtonClick = prop.FindPropertyRelative ("onButtonClick");

		
		EditorGUILayout.PropertyField (disableModifiedControls, true);

		if (notReady ()) {
			EditorGUILayout.PropertyField (background, true);
			EditorGUILayout.PropertyField (button, true);
		} else {
			EditorGUILayout.PropertyField (backgroundColor, true);
			EditorGUILayout.PropertyField (onButtonClick, true);
		}
	}
}