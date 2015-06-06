#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(APChoiceSystem))]
[CanEditMultipleObjects]
public class APChoiceSystemUnity : Editor {	
	SerializedProperty assetList = null;
	SerializedProperty tabList = null;
	SerializedProperty selectedTabIndex = null;
	SerializedProperty shouldResetAssets = null;
	SerializedProperty shouldResetHotKeys = null;
	List<string> extractedTabNameList;
	
	void OnEnable() {
		assetList = serializedObject.FindProperty ("assetList");
		tabList = serializedObject.FindProperty ("tabList");
		
		EditorPrefs.SetInt (APGlobals.SelectedKey, (int)KeyCode.None);
		
		extractedTabNameList = new List<string> ();
		
		selectedTabIndex = serializedObject.FindProperty ("selectedTabIndex");
		shouldResetAssets = serializedObject.FindProperty ("shouldResetAssets");
		shouldResetHotKeys = serializedObject.FindProperty ("shouldResetHotKeys");
	}
	
	void CreateTabSelection () {
		extractedTabNameList.Clear ();
		for (int index = 0; index < tabList.arraySize; index++) {
			extractedTabNameList.Add (tabList.GetArrayElementAtIndex (index).FindPropertyRelative("name").stringValue);
		}
		
		int selectedTabNumber = EditorPrefs.GetInt (APGlobals.SelectedTab);
		
		if (extractedTabNameList.Count > 0) {
			selectedTabNumber = GUILayout.SelectionGrid (selectedTabNumber, extractedTabNameList.ToArray(), extractedTabNameList.Count);
			serializedObject.ApplyModifiedProperties();
			
			selectedTabIndex.intValue = selectedTabNumber;
			
			serializedObject.Update ();
		}
		
		EditorPrefs.SetInt (APGlobals.SelectedTab, selectedTabNumber);	
	}
	
	void CreateAssetSelection () {
		for (int index = 0; index < assetList.arraySize; index++) {
			var tabName = assetList.GetArrayElementAtIndex (index).FindPropertyRelative("tab").stringValue;
			
			if(extractedTabNameList.Count < selectedTabIndex.intValue) {
				return;
			}
			
			if(tabName == extractedTabNameList[selectedTabIndex.intValue]) {
				EditorGUILayout.BeginVertical ();
				EditorGUILayout.PropertyField (assetList.GetArrayElementAtIndex (index), true);
				
				EditorGUILayout.EndVertical ();
			}
		}
	}
	
	void CreateResetButtons () {
		GUILayout.BeginHorizontal ();
		
		if (GUILayout.Button ("Reset Data")) {
			shouldResetAssets.boolValue = true;
		}
		
		if (GUILayout.Button ("Reset Keys")) {
			shouldResetHotKeys.boolValue = true;
		}
		
		GUILayout.EndHorizontal ();
	}
	
	Vector2 scrollPosition = Vector2.zero;
	public override void OnInspectorGUI() {
		serializedObject.Update ();
		
		var defaultStyle = new GUIStyle ();
		GUILayout.Label ("Asset Count: " + assetList.arraySize.ToString (), defaultStyle);
		
		CreateTabSelection ();
		CreateAssetSelection ();
		
		CreateResetButtons ();
		
		serializedObject.ApplyModifiedProperties ();
	}
}

#endif