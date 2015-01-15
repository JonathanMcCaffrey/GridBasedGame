using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(AssetPlacementChoiceSystem))]
[CanEditMultipleObjects]
public class AssetPlacementChoiceSystemUnity : Editor {	
	SerializedProperty assetList = null;
	SerializedProperty tabList = null;
	SerializedProperty selectedKey = null;
	SerializedProperty selectedTab = null;
	SerializedProperty shouldReset = null;
	
	int keyValue = -1;
	
	void OnEnable() {
		assetList = serializedObject.FindProperty ("assetList");
		tabList = serializedObject.FindProperty ("tabList");
		selectedKey = serializedObject.FindProperty ("selectedKey");
		selectedTab = serializedObject.FindProperty ("selectedTab");
		shouldReset = serializedObject.FindProperty ("shouldReset");
	}
	
	void CreateTabSelection () {
		List<string> extractedTabNameList = new List<string> ();
		for (int index = 0; index < tabList.arraySize; index++) {
			extractedTabNameList.Add (tabList.GetArrayElementAtIndex (index).FindPropertyRelative("name").stringValue);
		}
		
		int selectedTabNumber = EditorPrefs.GetInt (AssetPlacementKeys.SelectedTabKey);
		
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
		
		EditorPrefs.SetInt (AssetPlacementKeys.SelectedTabKey, selectedTabNumber);
		
	}
	
	void CreateAssetSelection () {
		for (int index = 0; index < assetList.arraySize; index++) {
			var tabName = assetList.GetArrayElementAtIndex (index).FindPropertyRelative("tab").stringValue;
			
			if(selectedTab != null && tabName == selectedTab.FindPropertyRelative("name").stringValue) {
				EditorGUILayout.BeginVertical ();
				EditorGUILayout.PropertyField (assetList.GetArrayElementAtIndex (index), true);
				
				if(assetList.GetArrayElementAtIndex (index).FindPropertyRelative("gameObject").objectReferenceValue == null) {
					string fixedPath = assetList.GetArrayElementAtIndex (index).FindPropertyRelative("filePath").stringValue; 
					fixedPath = fixedPath.Replace('\\', '/');
					
					var prefab = AssetDatabase.LoadAssetAtPath(fixedPath, typeof(GameObject)) as GameObject;
					assetList.GetArrayElementAtIndex (index).FindPropertyRelative("gameObject").objectReferenceValue =  prefab;
				}
				
				EditorGUILayout.EndVertical ();
			}
		}
	}
	
	void UpdateSelectedKey () {
		if (keyValue != -1) {
			selectedKey.intValue = keyValue;
		}
	}
	
	void CreateResetButton () {
		if (GUILayout.Button ("Reset")) {
			shouldReset.boolValue = true;
		}
	}
	
	public override void OnInspectorGUI() {
		serializedObject.Update ();
		
		GUILayout.Label ("Asset Count: " + assetList.arraySize.ToString ());
		GUILayout.Label ("Selected Key: " +((KeyCode)selectedKey.intValue).ToString ());
		
		CreateTabSelection ();
		CreateAssetSelection ();
		CreateResetButton ();
		
		UpdateSelectedKey ();
		
		serializedObject.ApplyModifiedProperties ();
	}
	
	public void OnSceneGUI() {
		if (Event.current.keyCode != KeyCode.None) {
			keyValue = (int)Event.current.keyCode;
			foreach(var assetData in AssetPlacementChoiceSystem.instance.assetList) {
				if(assetData.keyCode == Event.current.keyCode) { 
					EditorPrefs.SetInt (AssetPlacementKeys.SelectedAssetNumber, AssetPlacementKeys.HotKeySelectionEnabled);
					return;
				}
			}
		}
	}
}

