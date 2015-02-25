using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SceneSaver))]
[CanEditMultipleObjects]
public class SceneSaverEditor : Editor {
	SerializedProperty selectedNode;
	SerializedProperty fileName;
	SerializedProperty enemyType;
	SerializedProperty hotKey;
	
	void OnEnable() {
		selectedNode = serializedObject.FindProperty ("selectedNode");
		fileName = serializedObject.FindProperty ("fileName");
		
	}
	
	public override void OnInspectorGUI() {
		serializedObject.Update ();
		
		EditorGUILayout.PropertyField (selectedNode, true);
		EditorGUILayout.PropertyField (fileName, true);
		
		if (GUILayout.Button ("Save")) {
			if(SceneSaver.instance) {
				SceneSaver.SaveSelectedNode();
			}
			
		}
		
		if (GUILayout.Button ("Load")) {
			SceneSaver.LoadNode();
		}
		
		
		serializedObject.ApplyModifiedProperties ();
	}
}
