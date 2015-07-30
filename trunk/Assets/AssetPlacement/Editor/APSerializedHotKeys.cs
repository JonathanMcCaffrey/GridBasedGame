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
	[MenuItem( APGlobals.CommandPath + "Hot Keys/Space #space")]
#else
	[MenuItem( APGlobals.CommandPath + "Hot Keys/Space &_Space")]
#endif
	public static void SelectItemSpace() {
		EditorPrefs.SetInt (APGlobals.SelectedKey, (int)KeyCode.Space); 
		EditorPrefs.SetInt (APGlobals.SelectedAssetNumber, APGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.Space);
	}

#if UNITY_EDITOR_OSX
	[MenuItem( APGlobals.CommandPath + "Hot Keys/Alpha0 #alpha0")]
#else
	[MenuItem( APGlobals.CommandPath + "Hot Keys/Alpha0 &_Alpha0")]
#endif
	public static void SelectItemAlpha0() {
		EditorPrefs.SetInt (APGlobals.SelectedKey, (int)KeyCode.Alpha0); 
		EditorPrefs.SetInt (APGlobals.SelectedAssetNumber, APGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.Alpha0);
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
	[MenuItem( APGlobals.CommandPath + "Hot Keys/S #s")]
#else
	[MenuItem( APGlobals.CommandPath + "Hot Keys/S &_S")]
#endif
	public static void SelectItemS() {
		EditorPrefs.SetInt (APGlobals.SelectedKey, (int)KeyCode.S); 
		EditorPrefs.SetInt (APGlobals.SelectedAssetNumber, APGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.S);
	}

#if UNITY_EDITOR_OSX
	[MenuItem( APGlobals.CommandPath + "Hot Keys/D #d")]
#else
	[MenuItem( APGlobals.CommandPath + "Hot Keys/D &_D")]
#endif
	public static void SelectItemD() {
		EditorPrefs.SetInt (APGlobals.SelectedKey, (int)KeyCode.D); 
		EditorPrefs.SetInt (APGlobals.SelectedAssetNumber, APGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.D);
	}

#if UNITY_EDITOR_OSX
	[MenuItem( APGlobals.CommandPath + "Hot Keys/F #f")]
#else
	[MenuItem( APGlobals.CommandPath + "Hot Keys/F &_F")]
#endif
	public static void SelectItemF() {
		EditorPrefs.SetInt (APGlobals.SelectedKey, (int)KeyCode.F); 
		EditorPrefs.SetInt (APGlobals.SelectedAssetNumber, APGlobals.HotKeySelectionEnabled);
		RefreshSelectedKey(KeyCode.F);
	}

} 
#endif