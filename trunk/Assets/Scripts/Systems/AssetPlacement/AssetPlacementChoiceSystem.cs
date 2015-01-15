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
	public bool shouldReset = false;
	
	public static AssetPlacementData selectedAsset = null; 
	
	public List<AssetPlacementData> assetList = new List<AssetPlacementData>();
	public List<TabPlacementData> tabList = new List<TabPlacementData>();
	//Duplicated tab name data for ease of use only. Otherwise, use tabList
	public List<string> tabNames = new List<string>();
	
	public TabPlacementData selectedTab = null;
	
	private string folderName = "PlacementAssets";
	private string FolderPath() { 		
		return Application.dataPath + "\\" + folderName;
	}
	
	static private Dictionary<string, GameObject> tabContainerDictionary = new Dictionary<string, GameObject> (); 
	static public Dictionary<string, GameObject> TabContainerDictionary {
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
	
	void RefreshTabContainers () {
		string mainContainerName = "PlacedAssets";
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
		var selectedAssetNumber = EditorPrefs.GetInt (AssetPlacementKeys.SelectedAssetNumber);
		if (selectedAssetNumber != AssetPlacementKeys.HotKeySelectionEnabled) {
			selectedAsset = assetList [selectedAssetNumber];
			return true;
		}
		return false;
	}
	
	void ByHotKeySelection () {
		int index = 0;
		foreach (AssetPlacementData data in assetList) {
			if (selectedTab != null && data.tab == selectedTab.name) {
				if (data.keyCode == (KeyCode)EditorPrefs.GetInt(AssetPlacementKeys.SelectedKey)) {
					selectedAsset = data;
					
					EditorPrefs.SetInt (AssetPlacementKeys.SelectedAssetNumber, index);
				}
			}
			
			index++;
		}
	}
	
	void UpdateSelectedAsset () {
		var hasFoundAsset = ByButtonSelection (); 
		if (hasFoundAsset) {
			return;
		} else {
			ByHotKeySelection ();
		}
	}
	
	public void OnDrawGizmos() {
		instance = this;
		
		if (shouldReset) {
			shouldReset = false;
			WipeData ();
			LoadData();
			RefreshTabContainers ();
		}
		
		UpdateSelectedAsset ();
	}
}
