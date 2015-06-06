#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class APTabPlacementData {
	public string filePath;
	public string name;
	public int number;
	public GameObject container;
	
	public APTabPlacementData(string filePath = "", string name = "", int number = 0) {
		this.filePath = filePath;
		this.name = name;
		this.number = number;
	}
}

#endif