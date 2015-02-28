using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO Wrap this since it will probably break mobile
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

using UnityEditor;

public class AssetPlacementChoiceSystem : MonoBehaviour {
	public bool shouldResetAssets = true;
	public bool shouldResetHotKeys = true;
	
	public static AssetPlacementData selectedAsset = null; 
	
	public List<AssetPlacementData> assetList = new List<AssetPlacementData>();
	public List<TabPlacementData> tabList = new List<TabPlacementData>();
	//Duplicated tab name data for ease of use only. Otherwise, use tabList
	public List<string> tabNames = new List<string>();
	
	public TabPlacementData selectedTab = null;
	
	private string folderName = AssetPlacementGlobals.AssetPathPath + "PlacementAssets";
	private string FolderPath() { 		
		return Application.dataPath + "\\" + folderName;
	}
	
	private Dictionary<string, GameObject> tabContainerDictionary = new Dictionary<string, GameObject> (); 
	public Dictionary<string, GameObject> TabContainerDictionary {
		get { return tabContainerDictionary; }
	}

	public static AssetPlacementChoiceSystem instance = null;
	void Awake() {
		if (instance && instance != this) {
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
	}
	
	void LoadTabs () {
		var tabPaths = Directory.GetDirectories (FolderPath ());
		foreach (var filePath in tabPaths) {
			var name = filePath.Remove (0, FolderPath ().Length + 1);
			tabList.Add (new TabPlacementData(filePath, name));
			tabNames.Add(name);
		}
	}
	
	void LoadAssets (string searchedExtension = ".prefab"){
		if (assetList.Count == 0) {
			foreach (TabPlacementData tabData in tabList) {
				var filePaths = Directory.GetFiles (tabData.filePath);
				foreach (string filePath in filePaths) {
					var name = filePath.Remove (0, FolderPath ().Length + 1);
					var localPath = "Assets\\" + filePath.Remove (0, FolderPath ().Length - folderName.Length);
					
					if (name.EndsWith (searchedExtension)) {
						var assetData = new AssetPlacementData (localPath, name, tabData.name);
						assetList.Add (assetData);
						
						string fixedPath = localPath; 
						fixedPath = fixedPath.Replace('\\', '/');
						
						var prefab = AssetDatabase.LoadAssetAtPath(fixedPath, typeof(GameObject)) as GameObject;
						assetData.gameObject = prefab;
						
					}
					
				}
			}
		}
	}
	
	private void LoadData() {
		if (!Directory.Exists (FolderPath ())) {
			Directory.CreateDirectory (FolderPath ());
		}
		
		LoadTabs ();
		LoadAssets ();
	}
	
	void WipeData () {
		assetList.Clear ();
		tabList.Clear ();
	}
	
	public void RefreshTabContainers () {
		//TODO Put all these names into AssetPlacementKeys
		string mainContainerName = "AP.PlacedAssets";
		var placedAssetsContainer = GameObject.Find (mainContainerName);
		if (!placedAssetsContainer) {
			placedAssetsContainer = new GameObject (mainContainerName);
		}
		
		tabContainerDictionary.Clear ();
		foreach (var tab in tabList) {
			var tabContainerName = mainContainerName + "." + tab.name;
			var tabContainer = GameObject.Find (tabContainerName);
			if (!tabContainer) {
				tabContainer = new GameObject (tabContainerName);
			}
			tabContainer.transform.parent = placedAssetsContainer.transform;
			tabContainerDictionary.Add(tab.name, tabContainer);
		}
	}
	
	bool ByButtonSelection () {
		var selectedAssetNumber = EditorPrefs.GetInt (AssetPlacementGlobals.SelectedAssetNumber);
		if (selectedAssetNumber != AssetPlacementGlobals.HotKeySelectionEnabled) {
			selectedAsset = assetList [selectedAssetNumber];
			return true;
		}
		return false;
	}
	
	void ByHotKeySelection () {
		int index = 0;
		foreach (AssetPlacementData data in assetList) {
			if (selectedTab != null && data.tab == selectedTab.name) {
				if (data.keyCode == (KeyCode)EditorPrefs.GetInt(AssetPlacementGlobals.SelectedKey)) {
					selectedAsset = data;
					
					EditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, index);
				}
			}
			
			index++;
		}
	}
	
	void UpdateSelectedAsset () {
		if (assetList.Count == 0) {
			return;
		}
		
		var hasFoundAsset = ByButtonSelection (); 
		if (hasFoundAsset) {
			return;
		} else {
			ByHotKeySelection ();
		}
	}
	
	void RefreshSelectedTab () {
		foreach (var tabData in tabList) {
			if (tabData.name == EditorPrefs.GetString (AssetPlacementGlobals.SelectedTab)) {
				selectedTab = tabData;
			}
		}
	}
	
	public void Refresh () {
		if (shouldResetAssets) {
			
			shouldResetAssets = false;
			
			WipeData ();
			LoadData ();
			RefreshTabContainers ();
			RefreshSelectedTab ();
		}
	}
	
	public void OnDrawGizmos() {
		instance = this;
		
		if (EditorPrefs.GetBool (AssetPlacementGlobals.ShouldRefreshHotkeys)) {
			EditorPrefs.SetBool (AssetPlacementGlobals.ShouldRefreshHotkeys, false);
			shouldResetHotKeys = true;
		}
		
		if (shouldResetHotKeys) {
			shouldResetHotKeys = false;
			SaveAllHotKeys();
		}
		
		if (shouldResetAssets) {
			Refresh ();
		}
		
		UpdateSelectedAsset ();
	}
	
	string refreshSelectedKeyFunctionString = "\n\tstatic void RefreshSelectedKey (KeyCode hotkeyCode) {" +
		"\n\t\tif (hotkeyCode != KeyCode.None) {" +
			"\n\t\t\tint index = 0;" +
			"\n\t\t\tforeach (var assetData in AssetPlacementChoiceSystem.instance.assetList) {" +
			"\n\t\t\t\tif(AssetPlacementChoiceSystem.instance.selectedTab.name == assetData.tab) {" +
			"\n\t\t\t\t\tif (assetData.keyCode == hotkeyCode) {" +
			"\n\t\t\t\t\t\tEditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, index);" +
			"\n\t\t\t\t\t\tEditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)hotkeyCode);" +
			"\n\n\t\t\t\t\t\tif(AssetPlacementChoiceSystem.instance) {" +
			"\n\t\t\t\t\t\t\tAssetPlacementChoiceSystem.instance.OnDrawGizmos();" +
			"\n\t\t\t\t\t\t}" +
			"\n\n\t\t\t\t\t\treturn;" +
			"\n\t\t\t\t\t}" +
			"\n\t\t\t\t}" +
			"\n\t\t\tindex++;" +
			"\n\t\t\t}" +
			"\n\t\t}" +
			"\n\t}";
	
	public void SaveAllHotKeys() {
		var directoryPath = Application.dataPath + AssetPlacementGlobals.HotKeysPath;
		string content = "//This code is generated dynamically. Don't edit\nusing UnityEditor; \nusing UnityEngine; \n\npublic class AssetPlacementSerializedHotKeys : EditorWindow {";
		
		content += refreshSelectedKeyFunctionString;
		
		Dictionary<KeyCode, string> keyCodeList = new Dictionary<KeyCode, string>();
		
		foreach (var asset in assetList) {
			if(!keyCodeList.ContainsKey(asset.keyCode)) {
				var keyString = asset.keyCode.ToString();
				var text = 
					"\n\n\t[MenuItem( AssetPlacementGlobals.CommandPath + \"Hot Keys/" + keyString + " &_" + keyString + "\")]" +
						"\n\tpublic static void SelectItem" + keyString + "() {" +
						"\n\t\tEditorPrefs.SetInt (AssetPlacementGlobals.SelectedKey, (int)KeyCode." + keyString + "); " +
						"\n\t\tEditorPrefs.SetInt (AssetPlacementGlobals.SelectedAssetNumber, AssetPlacementGlobals.HotKeySelectionEnabled);" +
						"\n\t\tRefreshSelectedKey(KeyCode." + keyString + ");" +
						//"\n\t\tDebug.Log(\"" + keyString + "\");" +
						"\n\t}\n";
				
				content += text;
				
				keyCodeList.Add(asset.keyCode, asset.name);
			}
		}
		
		content += "\n}";
		
		File.WriteAllText(directoryPath, content);
		
		//TODO Was trying to refresh load this here with AssetData, but didn't work. Probably another method would work
	}
}
