using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[InitializeOnLoad]
public class AssetPlacement : EditorWindow {
	private static bool doSnapUpdate = true;
	
	static AssetPlacement() {
		EditorApplication.update += Update;	
	}
	
	public void OnGUI() {
		if (Event.current.keyCode != KeyCode.None) {
			int index = 0;
			foreach(var assetData in AssetPlacementChoiceSystem.instance.assetList) {
				if(assetData.keyCode == Event.current.keyCode) { 
					EditorPrefs.SetInt (AssetPlacementKeys.SelectedAssetNumber, index);
					return;
				}
				index++;
			}
		}
	}
	
	public void OnSceneGUI() {
		if (Event.current.keyCode != KeyCode.None) {
			int index = 0;
			foreach(var assetData in AssetPlacementChoiceSystem.instance.assetList) {
				if(assetData.keyCode == Event.current.keyCode) { 
					EditorPrefs.SetInt (AssetPlacementKeys.SelectedAssetNumber, index);
					return;
				}
				index++;
			}
		}
	}
	
	public void OnInspectorGUI() {
		if (Event.current.keyCode != KeyCode.None) {
			int index = 0;
			foreach(var assetData in AssetPlacementChoiceSystem.instance.assetList) {
				if(assetData.keyCode == Event.current.keyCode) { 
					EditorPrefs.SetInt (AssetPlacementKeys.SelectedAssetNumber, index);
					return;
				}
				index++;
			}
		}
	}
	
	public static void Update() {
		doSnapUpdate = EditorPrefs.GetBool (AssetPlacementKeys.SnapUpdate, false);
		
		/*
		if (Event.current && Event.current.keyCode != KeyCode.None) {
			int index = 0;
			foreach(var assetData in AssetPlacementChoiceSystem.instance.assetList) {
				if(assetData.keyCode == Event.current.keyCode) { 
					EditorPrefs.SetInt (AssetPlacementKeys.SelectedAssetNumber, index);
					return;
				}
				index++;
			}
		}*/
		
		if (AssetPlacementWindow.instance) {
			AssetPlacementWindow.instance.Repaint();
			
		}
		
		if (AssetPlacementChoiceSystem.instance) {
			//	AssetPlacementChoiceSystem.instance.OnDrawGizmos();	
			
		}
	}
}