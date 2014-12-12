using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Tile))]
[CanEditMultipleObjects]
public class TileEditor : Editor {
	SerializedProperty editorType;
	SerializedProperty collidableType;
	SerializedProperty enemyType;
	SerializedProperty hotKey;
	
	void OnEnable() {
		editorType = serializedObject.FindProperty ("mEditorType");
		collidableType = serializedObject.FindProperty ("mCollidableType");
		enemyType = serializedObject.FindProperty ("mEnemyType");
		hotKey = serializedObject.FindProperty ("mHotKey");
	}
	
	public override void OnInspectorGUI() {
		serializedObject.Update ();

		EditorGUILayout.PropertyField (editorType, true);
		EditorGUILayout.PropertyField (collidableType, true);
		
		if (collidableType.enumValueIndex == (int)Tile.CollidableType.Enemy) {
			EditorGUILayout.PropertyField (enemyType, true);	
		}

		EditorGUILayout.PropertyField (hotKey, true);

		serializedObject.ApplyModifiedProperties ();
	}
}
