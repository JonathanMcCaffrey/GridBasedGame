//This code is generated dynamically. Don't edit
using UnityEditor; 
using UnityEngine; 

public class AssetPlacementSerializedHotKeys : EditorWindow {
	static void RefreshSelectedKey (KeyCode hotkeyCode) {
		if (hotkeyCode != KeyCode.None) {
			int index = 0;
			foreach (var assetData in AssetPlacementChoiceSystem.instance.assetList) {
				if(AssetPlacementChoiceSystem.instance.selectedTab.name == assetData.tab) {
					if (assetData.keyCode == hotkeyCode) {
						EditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, index);
						EditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)hotkeyCode);

						if(AssetPlacementChoiceSystem.instance) {
							AssetPlacementChoiceSystem.instance.OnDrawGizmos();
						}

						return;
					}
				}
			index++;
			}
		}
	}

	[MenuItem( AssetPlacementGlobals.CommandPath + "Hot Keys/None &_None")]
	public static void SelectItemNone() {
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)KeyCode.None); 
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, AssetPlacementGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.None);
	}


	[MenuItem( AssetPlacementGlobals.CommandPath + "Hot Keys/Alpha1 &_Alpha1")]
	public static void SelectItemAlpha1() {
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)KeyCode.Alpha1); 
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, AssetPlacementGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.Alpha1);
	}


	[MenuItem( AssetPlacementGlobals.CommandPath + "Hot Keys/Alpha2 &_Alpha2")]
	public static void SelectItemAlpha2() {
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)KeyCode.Alpha2); 
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, AssetPlacementGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.Alpha2);
	}


	[MenuItem( AssetPlacementGlobals.CommandPath + "Hot Keys/Alpha3 &_Alpha3")]
	public static void SelectItemAlpha3() {
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)KeyCode.Alpha3); 
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, AssetPlacementGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.Alpha3);
	}


	[MenuItem( AssetPlacementGlobals.CommandPath + "Hot Keys/Alpha4 &_Alpha4")]
	public static void SelectItemAlpha4() {
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)KeyCode.Alpha4); 
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, AssetPlacementGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.Alpha4);
	}


	[MenuItem( AssetPlacementGlobals.CommandPath + "Hot Keys/Alpha5 &_Alpha5")]
	public static void SelectItemAlpha5() {
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)KeyCode.Alpha5); 
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, AssetPlacementGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.Alpha5);
	}


	[MenuItem( AssetPlacementGlobals.CommandPath + "Hot Keys/Alpha6 &_Alpha6")]
	public static void SelectItemAlpha6() {
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)KeyCode.Alpha6); 
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, AssetPlacementGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.Alpha6);
	}

}