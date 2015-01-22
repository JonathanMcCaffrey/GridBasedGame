using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(AssetPlacementData))]
public class AssetPlacementDataUnity : PropertyDrawer {
	public bool shouldLoadKey = true;
	public void OnEnable() {
		shouldLoadKey = true;
	}
	
	public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label) {
		SerializedProperty keyCode = prop.FindPropertyRelative ("keyCode");
		SerializedProperty name = prop.FindPropertyRelative ("name");
		SerializedProperty shouldRefresh = prop.FindPropertyRelative ("shouldRefresh");
		
		if (shouldRefresh.boolValue && name.stringValue != "") {
			shouldRefresh.boolValue = false;
			var foundValue = EditorPrefs.GetInt (AssetPlacementGlobals.SavedHotkey + name.stringValue);
			if(foundValue != 0) {
				keyCode.enumValueIndex = foundValue;
			}
		}
		
		string fixedLabel = name.stringValue.TrimEnd (".prefab".ToCharArray ());
		
		GUI.Label (new Rect (rect.x, rect.y, rect.width * 0.60f, rect.height), fixedLabel);
		EditorGUI.PropertyField (new Rect(rect.width * 0.65f, rect.y, rect.width * 0.35f, rect.height), keyCode, GUIContent.none);
		
		EditorPrefs.SetInt (AssetPlacementGlobals.SavedHotkey + name.stringValue, keyCode.enumValueIndex);
	}
}