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

	[MenuItem( "Window/Asset Placement Window/KeyCode None _None")]
	public static void SelectItemNone() {
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)KeyCode.None); 
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, AssetPlacementGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.None);
	}


	[MenuItem( "Window/Asset Placement Window/KeyCode Alpha1 _Alpha1")]
	public static void SelectItemAlpha1() {
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)KeyCode.Alpha1); 
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, AssetPlacementGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.Alpha1);
	}


	[MenuItem( "Window/Asset Placement Window/KeyCode Q _Q")]
	public static void SelectItemQ() {
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)KeyCode.Q); 
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, AssetPlacementGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.Q);
	}


	[MenuItem( "Window/Asset Placement Window/KeyCode Alpha3 _Alpha3")]
	public static void SelectItemAlpha3() {
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)KeyCode.Alpha3); 
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, AssetPlacementGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.Alpha3);
	}


	[MenuItem( "Window/Asset Placement Window/KeyCode Alpha4 _Alpha4")]
	public static void SelectItemAlpha4() {
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)KeyCode.Alpha4); 
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, AssetPlacementGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.Alpha4);
	}


	[MenuItem( "Window/Asset Placement Window/KeyCode W _W")]
	public static void SelectItemW() {
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)KeyCode.W); 
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, AssetPlacementGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.W);
	}


	[MenuItem( "Window/Asset Placement Window/KeyCode Alpha6 _Alpha6")]
	public static void SelectItemAlpha6() {
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)KeyCode.Alpha6); 
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, AssetPlacementGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.Alpha6);
	}

}