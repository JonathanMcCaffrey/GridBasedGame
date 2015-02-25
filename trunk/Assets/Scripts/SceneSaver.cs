using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using System;

//TODO Wrap this for mobile
using System.IO;

public class SceneSaver : MonoBehaviour {
	
	public GameObject nodeToSave = null;
	public string fileName = "temp";
	
	string FilePath() { 
		return "Assets" + "/";
	}
	
	void GetOnlyChildren (GameObject root, List<Transform> fixedChildren) {
		var children = root.GetComponentsInChildren<Transform> ();
		
		foreach (var child in children) {
			if (child.transform.parent == root.transform) {
				fixedChildren.Add (child);
			}
		}
	}	

	//TODO Make Cleaner
	void SaveSelectedNode () {
		if (!nodeToSave) {
			return;
		}
		
		var fixedChildren = new List<Transform> ();
		GetOnlyChildren (nodeToSave, fixedChildren);
		AssetNodeData rootNode = new AssetNodeData ("PlacementAssets");
		foreach (var subRoot in fixedChildren) {
			var subFixedChildren = new List<Transform> ();
			GetOnlyChildren (subRoot.gameObject, subFixedChildren);
			var splitName = subRoot.gameObject.name.Split ('.');
			var node = new AssetNodeData (splitName [2]);
			rootNode.children.Add (node);
			foreach (var subChild in subFixedChildren) {
				Debug.Log (subChild.parent.name + " + " + subChild.gameObject.name);
				node.children.Add (new AssetNodeData (subChild.gameObject.name));
			}
		}
		XmlSerializer xmlSerializer = new XmlSerializer (typeof(AssetNodeData));
		FileStream file = new FileStream (FilePath () + fileName + ".txt", FileMode.Create);
		xmlSerializer.Serialize (file, rootNode);
		file.Close ();
	}
	

	public void Awake() {
		SaveSelectedNode ();
	}
}


[Serializable]
[XmlRoot("node")]
public class AssetNodeData {
	[XmlAttribute("text")]
	public string text = "blank";
	
	[XmlArray("children"),XmlArrayItem("child")]
	public List<AssetNodeData> children = new List<AssetNodeData> ();
	
	public AssetNodeData() {
	}
	
	public AssetNodeData(string textData) {
		text = textData;
	}
}

