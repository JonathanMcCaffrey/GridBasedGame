#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO Wrap this since it will probably break mobile
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

using UnityEditor;

public class APChoiceSystem : MonoBehaviour {
	public bool shouldResetAssets = true;
	public bool shouldResetHotKeys = true;
	
	public static APData selectedAsset = null; 
	
	public List<APData> assetList = new List<APData>();
	public List<APTabPlacementData> tabList = new List<APTabPlacementData>();
	//Duplicated tab name data for ease of use only. Otherwise, use tabList
	public List<string> tabNames = new List<string>();
	
	public int selectedTabIndex = 0;
	public APTabPlacementData getSelectedTab() {
		return tabList [selectedTabIndex];
	}
	
	
	private string folderName = APGlobals.AssetPath + "PlacementAssets";
	private string FolderPath() { 		
		return Application.dataPath + "/" + folderName;
	}
	
	private Dictionary<string, GameObject> tabContainerDictionary = new Dictionary<string, GameObject> (); 
	public Dictionary<string, GameObject> TabContainerDictionary {
		get { return tabContainerDictionary; }
	}
	
	public static APChoiceSystem instance = null;
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
			tabList.Add (new APTabPlacementData(filePath, name));
			tabNames.Add(name);
		}
	}
	
	void LoadAssets (string searchedExtension = ".prefab"){
		if (assetList.Count == 0) {
			foreach (APTabPlacementData tabData in tabList) {
				var filePaths = Directory.GetFiles (tabData.filePath);
				foreach (string filePath in filePaths) {
					var name = filePath.Remove (0, FolderPath ().Length + 1);
					var localPath = "Assets/" + filePath.Remove (0, FolderPath ().Length - folderName.Length);
					
					if (name.EndsWith (searchedExtension)) {
						var assetData = new APData (localPath, name, tabData.name);
						assetList.Add (assetData);
						
						string fixedPath = localPath; 
						
						var prefab = AssetDatabase.LoadAssetAtPath(fixedPath, typeof(GameObject)) as GameObject;
						assetData.gameObject = prefab;
						
						var foundHotKey = EditorPrefs.GetString (APGlobals.SavedHotkeyDisplayName + assetData.name);
						if(foundHotKey.Length > 0) {
							foundHotKey = foundHotKey.Replace(" ", string.Empty);
							assetData.keyCode = (KeyCode)System.Enum.Parse( typeof( KeyCode ), foundHotKey );
						}
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
		var selectedAssetNumber = EditorPrefs.GetInt (APGlobals.SelectedAssetNumber);
		if (selectedAssetNumber != APGlobals.HotKeySelectionEnabled) {
			selectedAsset = assetList [selectedAssetNumber];
			return true;
		}
		return false;
	}
	
	void ByHotKeySelection () {
		int index = 0;
		foreach (APData data in assetList) {
			if (getSelectedTab() != null && data.tab == getSelectedTab().name) {
				if (data.keyCode == (KeyCode)EditorPrefs.GetInt(APGlobals.SelectedKey)) {
					selectedAsset = data;
					
					EditorPrefs.SetInt (APGlobals.SelectedAssetNumber, index);
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
		int index = 0;
		foreach (var tabData in tabList) {
			if (tabData.name == EditorPrefs.GetString (APGlobals.SelectedTab)) {
				selectedTabIndex = index;
			}
			index++;
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
		
		if (EditorPrefs.GetBool (APGlobals.ShouldRefreshHotkeys)) {
			EditorPrefs.SetBool (APGlobals.ShouldRefreshHotkeys, false);
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
			"\n\t\t\tforeach (var assetData in APChoiceSystem.instance.assetList) {" +
			"\n\t\t\t\tif(APChoiceSystem.instance.getSelectedTab().name == assetData.tab) {" +
			"\n\t\t\t\t\tif (assetData.keyCode == hotkeyCode) {" +
			"\n\t\t\t\t\t\tEditorPrefs.SetInt (APGlobals.SelectedAssetNumber, index);" +
			"\n\t\t\t\t\t\tEditorPrefs.SetInt (APGlobals.SelectedKey, (int)hotkeyCode);" +
			"\n\n\t\t\t\t\t\tif(APChoiceSystem.instance) {" +
			"\n\t\t\t\t\t\t\tAPChoiceSystem.instance.OnDrawGizmos();" +
			"\n\t\t\t\t\t\t}" +
			"\n\n\t\t\t\t\t\treturn;" +
			"\n\t\t\t\t\t}" +
			"\n\t\t\t\t}" +
			"\n\t\t\tindex++;" +
			"\n\t\t\t}" +
			"\n\t\t}" +
			"\n\t}";
	
	public void SaveAllHotKeys() {
		var directoryPath = Application.dataPath + APGlobals.HotKeysPath;
		string content = "#if UNITY_EDITOR\n//This code is generated dynamically. Don't edit here\n//Edit at APChoiceSystem.cs line: 195\nusing UnityEditor; \nusing UnityEngine; \n\npublic class APSerializedHotKeys : EditorWindow {";
		
		content += refreshSelectedKeyFunctionString;
		
		Dictionary<KeyCode, string> keyCodeList = new Dictionary<KeyCode, string>();
		
		foreach (var asset in assetList) {
			if(!keyCodeList.ContainsKey(asset.keyCode)) {
				var keyString = ((KeyCode)asset.keyCode).ToString();
				var text = 
					"\n#if UNITY_EDITOR_OSX" +
						"\n\t[MenuItem( APGlobals.CommandPath + \"Hot Keys/" + keyString + " #" + keyString.ToLower() + "\")]" +
						"\n#else" +
						"\n\t[MenuItem( APGlobals.CommandPath + \"Hot Keys/" + keyString + " &_" + keyString + "\")]" +
						"\n#endif" +
						"\n\tpublic static void SelectItem" + keyString + "() {" +
						"\n\t\tEditorPrefs.SetInt (APGlobals.SelectedKey, (int)KeyCode." + keyString + "); " +
						"\n\t\tEditorPrefs.SetInt (APGlobals.SelectedAssetNumber, APGlobals.HotKeySelectionEnabled);" +
						"\n\t\tRefreshSelectedKey(KeyCode." + keyString + ");" +
						//"\n\t\tDebug.Log(\"" + keyString + "\");" +
						"\n\t}\n";
				
				content += text;
				
				keyCodeList.Add(asset.keyCode, asset.name);
			}
		}
		
		content += "\n} \n#endif";
		
		File.WriteAllText(directoryPath, content);
	}
}

#endif