using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AssetPlacementData {
	public string filePath = "";
	public string name = "";
	public string tab = "";
	public KeyCode keyCode = KeyCode.A;
	public GameObject gameObject = null;
	
	public AssetPlacementData(string filePath, string name, string tab) {
		this.filePath = filePath;
		this.name = name;
		this.tab = tab;
	}
}
