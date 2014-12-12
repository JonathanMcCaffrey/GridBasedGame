using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Serialization;

public class LevelLayers : MonoBehaviour {
	
	public GameObject mDefaultTile = null;
	
	public GameObject mEnemyLayer = null;
	public GameObject mFloorLayer = null;
	public GameObject mWallLayer = null;
	
	public static LevelLayers instance = null;
	void Awake() {
		if (instance) {
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
	}
	
	public static void ClearLevel () {
		var wallList = instance.mWallLayer.GetComponentsInChildren<Tile> ();
		foreach (var temp in wallList) {
			Destroy (temp.gameObject);
		}
		
		var floorList = instance.mFloorLayer.GetComponentsInChildren<Tile> ();
		
		foreach (var temp in floorList) {
			Destroy (temp.gameObject);
		}
		
		var enemyList = instance.mEnemyLayer.GetComponentsInChildren<Tile> ();
		foreach (var temp in enemyList) {
			Destroy (temp.gameObject);
		}
	}
	
	void SetFromDataList (List<TileData> dataList, GameObject transformParent) {
		foreach (TileData tileData in dataList) {
			GameObject tempObject = Instantiate (mDefaultTile) as GameObject;
			Tile tempTile = tempObject.GetComponent<Tile> ();
			Texture2D texture2D = (Texture2D)Resources.Load (tileData.mTextureName);
			Rect rect = tempObject.GetComponent<SpriteRenderer> ().sprite.rect;
			tempObject.GetComponent<SpriteRenderer> ().sprite = Sprite.Create (texture2D, rect, new Vector2 (0.5f, 0.5f));
			tempObject.transform.position = new Vector3 (tileData.mX, tileData.mY);

			tempObject.transform.parent = transformParent.transform;
			if (tempTile) {
				tempTile.mCollidableType = tileData.mCollidableType;
			}
		}
	}	
	
	public void SetData(LevelLayersData aData) {
		LevelLayers.ClearLevel ();
		
		SetFromDataList (aData.mFloorDataList, LevelLayers.instance.mFloorLayer);
		SetFromDataList (aData.mWallDataList, LevelLayers.instance.mWallLayer);
	}
	
	public LevelLayersData GenerateData() {
		return new LevelLayersData (this);                        
	}
}

[Serializable]
[XmlRoot("levelLayersData")]
public class LevelLayersData {
	
	[XmlArray("enemies"),XmlArrayItem("enemy")]
	public List<TileData> mEnemyDataList = new List<TileData> ();
	[XmlArray("floors"),XmlArrayItem("floor")]
	public List<TileData> mFloorDataList = new List<TileData> ();
	[XmlArray("walls"),XmlArrayItem("wall")]
	public List<TileData> mWallDataList = new List<TileData> ();
	
	public LevelLayersData() {
	}
	
	public LevelLayersData(LevelLayers aDataSource) {
		var floorTilesFound = aDataSource.mFloorLayer.GetComponentsInChildren<Tile>();
		foreach(Tile foundTile in floorTilesFound) {
			mFloorDataList.Add(foundTile.GenerateData());
		}
		
		var wallTilesFound = aDataSource.mWallLayer.GetComponentsInChildren<Tile>();
		foreach(Tile foundTile in wallTilesFound) {
			mWallDataList.Add(foundTile.GenerateData());
		}
	}
}