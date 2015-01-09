using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ButtonIcon))]
[CanEditMultipleObjects]
public class ButtonIconUnity : Editor {
	
	SerializedProperty baseButton;
	SerializedProperty icon;
	SerializedProperty iconSprite;
	
	void OnEnable() {
		baseButton = serializedObject.FindProperty ("baseButton");
		icon = serializedObject.FindProperty ("icon");
		iconSprite = serializedObject.FindProperty ("iconSprite");
	}
	
	bool notReady() {
		return !icon.objectReferenceValue;
	}
	
	public override void OnInspectorGUI() {
		serializedObject.Update ();
		
		EditorGUILayout.PropertyField (baseButton, true);
		
		if (notReady ()) {
			EditorGUILayout.PropertyField (icon, true);
		} else {
			EditorGUILayout.PropertyField (iconSprite, true);
		}
		
		serializedObject.ApplyModifiedProperties ();
	}
}

