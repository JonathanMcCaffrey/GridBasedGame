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

	[MenuItem( AssetPlacementGlobals.CommandPath + "Hot Keys/None _None")]
	public static void SelectItemNone() {
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)KeyCode.None); 
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, AssetPlacementGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.None);
	}


	[MenuItem( AssetPlacementGlobals.CommandPath + "Hot Keys/Alpha1 _Alpha1")]
	public static void SelectItemAlpha1() {
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)KeyCode.Alpha1); 
		EditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, AssetPlacementGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.Alpha1);
	}

}