using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using System;

//TODO Wrap this for mobile
using System.IO;
using UnityEditor;



public class SceneSaver : MonoBehaviour {	
	public GameObject selectedNode = null;
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
		if (!selectedNode) {
			return;
		}
		
		var fixedChildren = new List<Transform> ();
		GetOnlyChildren (selectedNode, fixedChildren);
		AssetNodeData rootNode = new AssetNodeData ("PlacementAssets", Vector3.zero);
		foreach (var subRoot in fixedChildren) {
			var subFixedChildren = new List<Transform> ();
			GetOnlyChildren (subRoot.gameObject, subFixedChildren);
			var splitName = subRoot.gameObject.name.Split ('.');
			var node = new AssetNodeData (splitName [2], subRoot.gameObject.transform.localPosition);
			rootNode.children.Add (node);
			foreach (var subChild in subFixedChildren) {
				node.children.Add (new AssetNodeData (subChild.gameObject.name, subChild.gameObject.transform.localPosition));
			}
		}
		XmlSerializer xmlSerializer = new XmlSerializer (typeof(AssetNodeData));
		FileStream file = new FileStream (FilePath () + fileName + ".txt", FileMode.Create);
		xmlSerializer.Serialize (file, rootNode);
		file.Close ();
	}

	void LoadNode () {
		XmlSerializer xmlSerializer = new XmlSerializer (typeof(AssetNodeData));
		FileStream file = new FileStream (FilePath () + fileName + ".txt", FileMode.Open);
		AssetNodeData data = xmlSerializer.Deserialize (file) as AssetNodeData;
		file.Close ();
		GameObject rootLevel = null;
		rootLevel = GameObject.Find (data.text);
		if (!rootLevel) {
			rootLevel = new GameObject (data.text);
		}
		rootLevel.transform.localPosition = data.Position ();
		foreach (var dataNode in data.children) {
			GameObject subLevel = null;
			subLevel = GameObject.Find (dataNode.text);
			if (!subLevel) {
				subLevel = new GameObject (dataNode.text);
			}
			subLevel.transform.parent = rootLevel.transform;
			subLevel.transform.localPosition = dataNode.Position ();
			foreach (var childNode in dataNode.children) {
				//TODO Make cleaner
				string assetString = "Assets/PlacementAssets/" + dataNode.text + "/" + childNode.text + ".prefab";
				var asset = AssetDatabase.LoadAssetAtPath (assetString, typeof(GameObject)) as GameObject;
				if (asset) {
					var newObject = GameObject.Instantiate (asset) as GameObject;
					newObject.name = childNode.text;
					newObject.transform.parent = subLevel.transform;
					newObject.transform.localPosition = childNode.Position ();
				}
			}
		}
	}	
	
	public void Awake() {
		SaveSelectedNode ();
		LoadNode ();
	}
}


[Serializable]
[XmlRoot("node")]
public class AssetNodeData {
	[XmlAttribute("text")]
	public string text = "blank";
	
	[XmlAttribute("x")]
	public float x = 0;
	
	[XmlAttribute("y")]
	public float y = 0;
	
	[XmlAttribute("z")]
	public float z = 0;
	
	public Vector3 Position() {
		return new Vector3 (x, y, z);
	}
	
	[XmlArray("children"),XmlArrayItem("child")]
	public List<AssetNodeData> children = new List<AssetNodeData> ();
	
	public AssetNodeData() {
	}
	
	public AssetNodeData(string text, Vector3 position) {
		this.text = text;
		this.x = position.x;
		this.y = position.y;
		this.z = position.z;
	}
}

