#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class APData {
	public string filePath = "";
	public string name = "";
	public string tab = "";
	public KeyCode keyCode = KeyCode.None;
	
	public GameObject gameObject = null;
	
	public bool shouldRefresh = true;
	
	public APData(string filePath, string name, string tab) {
		this.filePath = filePath;
		this.name = name;
		this.tab = tab;
	}
}

#endif