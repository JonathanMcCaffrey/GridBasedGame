using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Serialization;

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
[XmlRoot("levelHeaderData")]
public class LevelHeaderData {
	[XmlAttribute("slotNumber")]
	public int mSlotNumber = 0;
	[XmlAttribute("name")]
	public string mName = "";
	[XmlAttribute("fileName")]
	public string mFileName = "";
	
	public LevelHeaderData() {
	}
	
	public LevelHeaderData(LevelHeader aDataSource) {
		mSlotNumber = aDataSource.mSlotNumber;
		mName = aDataSource.mName;
		mFileName = aDataSource.getSafeFileName ();
	}
}
