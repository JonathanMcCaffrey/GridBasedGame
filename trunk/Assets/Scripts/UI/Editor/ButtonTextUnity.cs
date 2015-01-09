using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ButtonText))]
[CanEditMultipleObjects]
public class ButtonTextUnity : Editor {
	
	SerializedProperty baseButton;
	SerializedProperty labelObject;
	SerializedProperty text;
	
	
	void OnEnable() {
		baseButton = serializedObject.FindProperty ("baseButton");
		labelObject = serializedObject.FindProperty ("labelObject");
		text = serializedObject.FindProperty ("text");
	}
	
	bool notReady() {
		return !labelObject.objectReferenceValue;
	}
	
	public override void OnInspectorGUI() {
		serializedObject.Update ();
		
		EditorGUILayout.PropertyField (baseButton, true);
		
		if (baseButton.FindPropertyRelative ("disableModifiedControls").boolValue) {
			serializedObject.ApplyModifiedProperties ();
			return;
		}
		
		if (notReady ()) {
			EditorGUILayout.PropertyField (labelObject, true);
		} else {
			text.stringValue = EditorGUILayout.TextField(text.stringValue);
		}
		
		serializedObject.ApplyModifiedProperties ();
	}
}