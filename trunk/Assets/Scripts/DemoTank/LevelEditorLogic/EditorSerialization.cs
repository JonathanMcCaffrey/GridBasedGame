using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;


public class DataPack {
	public LevelHeaderData mHeaderData = null;
	public LevelLayersData mLevelLayersData = null;
	
	public DataPack(LevelHeaderData aHeaderData, LevelLayersData aLayersData) {
		mHeaderData = aHeaderData;
		mLevelLayersData = aLayersData;
	}
}

public class EditorSerialization : MonoBehaviour, IKeyListener {
	Dictionary<KeyCode, int> mSlotList = new Dictionary<KeyCode, int>();
	
	List<LevelLayersData> mLayersData = new List<LevelLayersData>();
	public List<LevelLayersData> LayersData {
		get {
			return mLayersData;
		}
	}
	
	List<LevelHeaderData> mHeadersData = new List<LevelHeaderData>();
	public List<LevelHeaderData> HeadersData {
		get {
			return mHeadersData;
		}
	}	
	
	bool mIsSaving = false;
	
	KeyCode mSaveKey = KeyCode.KeypadPeriod;
	KeyCode mLoadKey = KeyCode.Keypad0;
	
	string mLevelLayersFolderName = "LevelLayersData";
	string mLevelHeaderFolderName = "LevelHeaderData";
	
	
	string FilePath() { 
		return Application.persistentDataPath + "/";
	}
	
	string LayersFilePath() { 
		return Application.persistentDataPath + "/" + mLevelLayersFolderName + "/";
	}
	
	string HeaderFilePath() { 
		return Application.persistentDataPath + "/" + mLevelHeaderFolderName + "/";
	}
	
	public static EditorSerialization instance = null;
	void Awake() {
		if (instance) {
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			instance = this;		
		}
	}
	
	void CreateFolders () {
		Debug.Log ("FilePath: " + FilePath () + mLevelLayersFolderName);

		if (!Directory.Exists (FilePath () + mLevelLayersFolderName)) {
			Directory.CreateDirectory (FilePath () + mLevelLayersFolderName);
		}
		
		if (!Directory.Exists (FilePath () + mLevelHeaderFolderName)) {
			Directory.CreateDirectory (FilePath () + mLevelHeaderFolderName);
		}
	}
	
	void SetComputerHotkeys () {
		mSlotList.Add (KeyCode.Keypad1, 1);
		mSlotList.Add (KeyCode.Keypad2, 2);
		mSlotList.Add (KeyCode.Keypad3, 3);
		mSlotList.Add (KeyCode.Keypad4, 4);
		mSlotList.Add (KeyCode.Keypad5, 5);
		mSlotList.Add (KeyCode.Keypad6, 6);
		mSlotList.Add (KeyCode.Keypad7, 7);
		mSlotList.Add (KeyCode.Keypad8, 8);
		mSlotList.Add (KeyCode.Keypad9, 9);
		Keys.AddListener (this);
	}
	
	void LoadLayerHeaders () {
		var layersPathList = Directory.GetFiles (HeaderFilePath ());
		foreach (string path in layersPathList) {
			
			var dataPack = LoadLayerFromDirectPath (path, false);
			
			mLayersData.Add (dataPack.mLevelLayersData);
			mHeadersData.Add (dataPack.mHeaderData);
		}
	}
	
	void Start() {
		CreateFolders ();
		SetComputerHotkeys (); 

		LoadLayerHeaders ();
	}
	
	
	public void Save(int aSlot) {
		#if UNITY_WP8
		
		LevelHeader.instance.mSlotNumber = aSlot;
		LevelHeaderData headerData = LevelHeader.instance.GenerateData ();
		
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(LevelHeaderData));
			FileStream file = new FileStream(HeaderFilePath () + aSlot.ToString () + ".txt", FileMode.Create);
			xmlSerializer.Serialize(file, headerData);
			file.Close();
		}
		
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(LevelLayersData));
			FileStream file = new FileStream(LayersFilePath() + headerData.mFileName + ".txt", FileMode.Create);
			xmlSerializer.Serialize(file, LevelLayers.instance.GenerateData());
			file.Close();
		}
		
		
		#else
		
		LevelHeader.instance.mSlotNumber = aSlot;
		LevelHeaderData headerData = LevelHeader.instance.GenerateData ();
		
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream file = File.Create (HeaderFilePath () + aSlot.ToString () + ".txt");
			binaryFormatter.Serialize (file, headerData);
			file.Close ();
		}
		
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream file = File.Create(LayersFilePath() + headerData.mFileName + ".txt");
			binaryFormatter.Serialize(file, LevelLayers.instance.GenerateData ());
			file.Close ();
		}
		#endif
	}
	
	DataPack LoadLayerFromDirectPath (string directPath, bool updateInstance = true) {
		#if UNITY_WP8
		
		
		if (File.Exists (directPath)) {
			LevelHeaderData headerData = null;
			
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(LevelHeaderData));
				FileStream file = new FileStream(directPath, FileMode.Open);
				headerData = xmlSerializer.Deserialize(file) as LevelHeaderData;
				file.Close();
			}
			
			LevelLayersData levelLayersData = null;
			
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(LevelLayersData));
				FileStream file = new FileStream(LayersFilePath() + headerData.mFileName + ".txt", FileMode.Open);
				levelLayersData = xmlSerializer.Deserialize(file) as LevelLayersData;
				file.Close();
			}
			
			if(updateInstance) {
				LevelHeader.instance.SetData (headerData);
				LevelLayers.instance.SetData(levelLayersData);
				
			}
			
			return new DataPack(headerData, levelLayersData);
		}
		
		#else
		
		if (File.Exists (directPath)) {
			LevelHeaderData headerData = null;
			
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter ();
				FileStream file = File.Open (directPath, FileMode.Open);
				headerData = (LevelHeaderData)binaryFormatter.Deserialize (file);
				file.Close ();
			}
			
			LevelLayersData levelLayersData = null;
			
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter ();
				FileStream file = File.Open (LayersFilePath() + headerData.mFileName + ".txt", FileMode.Open);
				levelLayersData = (LevelLayersData)binaryFormatter.Deserialize (file);
				file.Close ();
			}
			
			if(updateInstance) {
				LevelHeader.instance.SetData (headerData);
				LevelLayers.instance.SetData(levelLayersData);
			}
			
			return new DataPack(headerData, levelLayersData);
		}
		
		#endif
		
		return null;
	}	
	
	public DataPack LoadLayerFromSlot(int aSlot, bool updateInstance) {
		string directPath = HeaderFilePath() + aSlot.ToString () + ".txt";
		
		return LoadLayerFromDirectPath (directPath, updateInstance);
	}
	
	public void OnKeyPressed (KeyCode aKeyCode) {
		if (Input.GetKeyDown (mSaveKey)) {
			print ("Set to Save Mode");
			mIsSaving = true;
		} else if (Input.GetKeyDown (mLoadKey)) {
			print ("Set to Load Mode");
			mIsSaving = false;
		}
		
		if(!mSlotList.ContainsKey(aKeyCode)) {
			return;
		}
		
		if (mIsSaving) {
			Save(mSlotList[aKeyCode]);			
		} else {
			LoadLayerFromSlot(mSlotList[aKeyCode], true);
		}
	}
}
