#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(APData))]
public class APDataDrawer : PropertyDrawer {
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
			var foundValue = EditorPrefs.GetInt (APGlobals.SavedHotkeyDisplayIndex + name.stringValue);
			if(foundValue != 0) {
				keyCode.enumValueIndex = foundValue;
			}
		}
		
		string nameString = name.stringValue;
		
		string fixedLabel = "";
		
		if (nameString.Contains (".prefab")) {
			fixedLabel = nameString.Remove (nameString.Length - ".prefab".Length, ".prefab".Length);
		} else {
			fixedLabel = nameString;
		}

		var stringList = fixedLabel.Split ('/');
		fixedLabel = stringList[stringList.Length - 1];

		GUI.Label (new Rect (rect.x, rect.y, rect.width * 0.60f, rect.height), fixedLabel);
		EditorGUI.PropertyField (new Rect(rect.width * 0.65f, rect.y, rect.width * 0.35f, rect.height), keyCode, GUIContent.none);
		
		EditorPrefs.SetString (APGlobals.SavedHotkeyDisplayName + name.stringValue, keyCode.enumDisplayNames[keyCode.enumValueIndex]);
		
		EditorPrefs.SetInt (APGlobals.SavedHotkeyDisplayIndex + name.stringValue, keyCode.enumValueIndex);
	}
}

#endif