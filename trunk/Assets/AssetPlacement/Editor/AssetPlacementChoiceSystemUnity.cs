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
	SerializedProperty shouldReset = null;
	
	static int keyValue = (int)KeyCode.None;
	
	void OnEnable() {
		assetList = serializedObject.FindProperty ("assetList");
		tabList = serializedObject.FindProperty ("tabList");
		
		EditorPrefs.SetInt (AssetPlacementKeys.SelectedKey, (int)KeyCode.None);
		
		selectedTab = serializedObject.FindProperty ("selectedTab");
		shouldReset = serializedObject.FindProperty ("shouldReset");
	}
	
	void CreateTabSelection () {
		List<string> extractedTabNameList = new List<string> ();
		for (int index = 0; index < tabList.arraySize; index++) {
			extractedTabNameList.Add (tabList.GetArrayElementAtIndex (index).FindPropertyRelative("name").stringValue);
		}
		
		int selectedTabNumber = EditorPrefs.GetInt (AssetPlacementKeys.SelectedTab);
		
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
		
		EditorPrefs.SetInt (AssetPlacementKeys.SelectedTab, selectedTabNumber);	
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
	
	void CreateResetButton () {
		if (GUILayout.Button ("Reset")) {
			shouldReset.boolValue = true;
		}
	}

	Vector2 scrollPosition = Vector2.zero;
	public override void OnInspectorGUI() {
		serializedObject.Update ();

		var defaultStyle = new GUIStyle ();
		GUILayout.Label ("Asset Count: " + assetList.arraySize.ToString (), defaultStyle);
		GUILayout.Label ("Selected Key: " +((KeyCode)keyValue).ToString (), defaultStyle);
		
		CreateTabSelection ();

		CreateAssetSelection ();

		CreateResetButton ();
		
		serializedObject.ApplyModifiedProperties ();
	}
	
	static void RefreshSelectedKey () {
		EditorPrefs.SetInt (AssetPlacementKeys.SelectedKey, (int)Event.current.keyCode);
		if (Event.current.keyCode != KeyCode.None) {
			keyValue = (int)Event.current.keyCode;
			
			int index = 0;
			foreach (var assetData in AssetPlacementChoiceSystem.instance.assetList) {
				if(AssetPlacementChoiceSystem.instance.selectedTab.name == assetData.tab) {
					if (assetData.keyCode == Event.current.keyCode) {
						EditorPrefs.SetInt (AssetPlacementKeys.SelectedAssetNumber, index);
						return;
					}
				}
				index++;
			}
		}
	}
	
	public void OnGUI() {
		RefreshSelectedKey ();
	}
	
	public void OnSceneGUI() {
		RefreshSelectedKey ();
	}
}

