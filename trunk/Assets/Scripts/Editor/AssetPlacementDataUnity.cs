using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(AssetPlacementData))]
public class AssetPlacementDataUnity : PropertyDrawer {
	public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label) {
		SerializedProperty keyCode = prop.FindPropertyRelative ("keyCode");
		SerializedProperty name = prop.FindPropertyRelative ("name");
		
		GUI.Label (new Rect (rect.x, rect.y, rect.width / 2, rect.height), name.stringValue);
		EditorGUI.PropertyField (new Rect(rect.x + rect.width / 2, rect.y, rect.width / 2, rect.height), keyCode);
	}
}