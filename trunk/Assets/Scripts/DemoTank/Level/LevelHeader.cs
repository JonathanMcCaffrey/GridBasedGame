using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class LevelHeader : MonoBehaviour {

	public int mSlotNumber = 0;
	public string mName = "";
	public string mFileName = "";

	public string getSafeFileName() {
		return mFileName.Length > 0 ? mFileName : "Temp_" + mSlotNumber.ToString ();
	}

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
		mSlotNumber = aData.mSlotNumber;
		mFileName = aData.mFileName;
		mName = aData.mName;
	}

	public LevelHeaderData GenerateData() {
		return new LevelHeaderData (this);                        
	}
}

[Serializable]
public class LevelHeaderData {
	public int mSlotNumber = 0;
	public string mName = "";
	public string mFileName = "";

	public LevelHeaderData(LevelHeader aDataSource) {
		mSlotNumber = aDataSource.mSlotNumber;
		mName = aDataSource.mName;
		mFileName = aDataSource.getSafeFileName ();
	}
}
