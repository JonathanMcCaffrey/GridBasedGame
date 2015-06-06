#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(APPositionSystem))]
[CanEditMultipleObjects]
public class APPositionSystemEdtior : Editor {
	SerializedProperty marker;
	SerializedProperty xPosition;
	SerializedProperty yPosition;

	//Note: Offset of placement off mouse position. Re-enable if needed.
	//SerializedProperty adjustX;
	//SerializedProperty adjustY;

	SerializedProperty isMarkerActive;
	
	void OnEnable() {
		marker = serializedObject.FindProperty ("marker");
		xPosition = serializedObject.FindProperty ("xPosition");
		yPosition = serializedObject.FindProperty ("yPosition");

		//adjustX = serializedObject.FindProperty ("adjustX");
		//adjustY = serializedObject.FindProperty ("adjustY");
		
		isMarkerActive = serializedObject.FindProperty ("isMarkerActive");
	}
	
	bool notReady() {
		return !marker.objectReferenceValue;
	}
	
	public override void OnInspectorGUI() {
		serializedObject.Update ();
		
		if (notReady ()) {
			EditorGUILayout.PropertyField (marker, true);
		} else {
			GUILayout.Label ("Position [X: " + xPosition.floatValue + " Y: " + yPosition.floatValue + "]");
			
			//EditorGUILayout.PropertyField (adjustX, true);
			//EditorGUILayout.PropertyField (adjustY, true);
			
			EditorGUILayout.PropertyField (isMarkerActive, true);
		}
		
		serializedObject.ApplyModifiedProperties ();
	}
}

#endif
