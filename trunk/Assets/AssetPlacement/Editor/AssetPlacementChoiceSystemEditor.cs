using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(AssetPlacementChoiceSystem))]
[CanEditMultipleObjects]
public class AssetPlacementChoiceSystemUnity : Editor {	
	SerializedProperty assetList = null;
	SerializedProperty tabList = null;
	SerializedProperty selectedTab = null;
	SerializedProperty shouldResetAssets = null;
	SerializedProperty shouldResetHotKeys = null;
	
	void OnEnable() {
		assetList = serializedObject.FindProperty ("assetList");
		tabList = serializedObject.FindProperty ("tabList");
		
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)KeyCode.None);
		
		selectedTab = serializedObject.FindProperty ("selectedTab");
		shouldResetAssets = serializedObject.FindProperty ("shouldResetAssets");
		shouldResetHotKeys = serializedObject.FindProperty ("shouldResetHotKeys");
	}
	
	void CreateTabSelection () {
		List<string> extractedTabNameList = new List<string> ();
		for (int index = 0; index < tabList.arraySize; index++) {
			extractedTabNameList.Add (tabList.GetArrayElementAtIndex (index).FindPropertyRelative("name").stringValue);
		}
		
		int selectedTabNumber = EditorPrefs.GetInt (AssetPlacementGlobals.SelectedTab);
		
		if (extractedTabNameList.Count > 0) {
			
			selectedTabNumber = GUILayout.SelectionGrid (selectedTabNumber, extractedTabNameList.ToArray(), extractedTabNameList.Count);
			serializedObject.ApplyModifiedProperties();
			
			selectedTab.serializedObject.Update ();
			
			selectedTab.FindPropertyRelative("name").stringValue = tabList.GetArrayElementAtIndex(selectedTabNumber).FindPropertyRelative("name").stringValue;
			selectedTab.FindPropertyRelative("filePath").stringValue = tabList.GetArrayElementAtIndex(selectedTabNumber).FindPropertyRelative("filePath").stringValue;
			selectedTab.FindPropertyRelative("number").intValue = tabList.GetArrayElementAtIndex(selectedTabNumber).FindPropertyRelative("number").intValue;
			
			selectedTab.serializedObject.ApplyModifiedProperties();
			
			serializedObject.Update ();
		}
		
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedTab, selectedTabNumber);	
	}
	
	void CreateAssetSelection () {
		for (int index = 0; index < assetList.arraySize; index++) {
			var tabName = assetList.GetArrayElementAtIndex (index).FindPropertyRelative("tab").stringValue;
			
			if(selectedTab != null && tabName == selectedTab.FindPropertyRelative("name").stringValue) {
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

