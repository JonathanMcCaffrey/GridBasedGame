#if UNITY_EDITOR
//This code is generated dynamically. Don't edit here
//Edit at APChoiceSystem.cs line: 195
using UnityEditor; 
using UnityEngine; 

public class APSerializedHotKeys : EditorWindow {
	static void RefreshSelectedKey (KeyCode hotkeyCode) {
		if (hotkeyCode != KeyCode.None) {
			int index = 0;
			foreach (var assetData in APChoiceSystem.instance.assetList) {
				if(APChoiceSystem.instance.getSelectedTab().name == assetData.tab) {
					if (assetData.keyCode == hotkeyCode) {
						EditorPrefs.SetInt (APGlobals.SelectedAssetNumber, index);
						EditorPrefs.SetInt (APGlobals.SelectedKey, (int)hotkeyCode);

						if(APChoiceSystem.instance) {
							APChoiceSystem.instance.OnDrawGizmos();
						}

						return;
					}
				}
			index++;
			}
		}
	}
#if UNITY_EDITOR_OSX
	[MenuItem( APGlobals.CommandPath + "Hot Keys/None #none")]
#else
	[MenuItem( APGlobals.CommandPath + "Hot Keys/None &_None")]
#endif
	public static void SelectItemNone() {
		EditorPrefs.SetInt (APGlobals.SelectedKey, (int)KeyCode.None); 
		EditorPrefs.SetInt (APGlobals.SelectedAssetNumber, APGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.None);
	}

#if UNITY_EDITOR_OSX
	[MenuItem( APGlobals.CommandPath + "Hot Keys/Q #q")]
#else
	[MenuItem( APGlobals.CommandPath + "Hot Keys/Q &_Q")]
#endif
	public static void SelectItemQ() {
		EditorPrefs.SetInt (APGlobals.SelectedKey, (int)KeyCode.Q); 
		EditorPrefs.SetInt (APGlobals.SelectedAssetNumber, APGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.Q);
	}

#if UNITY_EDITOR_OSX
	[MenuItem( APGlobals.CommandPath + "Hot Keys/A #a")]
#else
	[MenuItem( APGlobals.CommandPath + "Hot Keys/A &_A")]
#endif
	public static void SelectItemA() {
		EditorPrefs.SetInt (APGlobals.SelectedKey, (int)KeyCode.A); 
		EditorPrefs.SetInt (APGlobals.SelectedAssetNumber, APGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.A);
	}

#if UNITY_EDITOR_OSX
	[MenuItem( APGlobals.CommandPath + "Hot Keys/E #e")]
#else
	[MenuItem( APGlobals.CommandPath + "Hot Keys/E &_E")]
#endif
	public static void SelectItemE() {
		EditorPrefs.SetInt (APGlobals.SelectedKey, (int)KeyCode.E); 
		EditorPrefs.SetInt (APGlobals.SelectedAssetNumber, APGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.E);
	}

#if UNITY_EDITOR_OSX
	[MenuItem( APGlobals.CommandPath + "Hot Keys/R #r")]
#else
	[MenuItem( APGlobals.CommandPath + "Hot Keys/R &_R")]
#endif
	public static void SelectItemR() {
		EditorPrefs.SetInt (APGlobals.SelectedKey, (int)KeyCode.R); 
		EditorPrefs.SetInt (APGlobals.SelectedAssetNumber, APGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.R);
	}

} 
#endif