using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TabPlacementData {
	public string filePath;
	public string name;
	public int number;
	public GameObject container;
	
	public TabPlacementData(string filePath = "", string name = "", int number = 0) {
		this.filePath = filePath;
		this.name = name;
		this.number = number;
	}
}

