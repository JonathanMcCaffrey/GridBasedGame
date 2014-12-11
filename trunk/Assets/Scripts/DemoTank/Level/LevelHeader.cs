using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class LevelHeader : MonoBehaviour {

	public int mSelectOrder = 0;
	public string mName = "";
	public string mFileName = "";

	public static LevelHeader instance = null;
	void Awake() {
		if (instance) {
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
	}

	public void SetData(LevelHeaderData aData) {
	}

	public LevelHeaderData GenerateData() {
		return new LevelHeaderData (this);                        
	}
}

[Serializable]
public class LevelHeaderData {
	public int mSelectOrder = 0;
	public string mName = "";
	public string mFileName = "";

	public LevelHeaderData(LevelHeader aDataSource) {

	}
}
