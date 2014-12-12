using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class EditorSave : MonoBehaviour, IKeyListener {
	Dictionary<KeyCode, int> mSlotList = new Dictionary<KeyCode, int>();
	
	List<LevelLayersData> mLayersData = new List<LevelLayersData>();
	public List<LevelLayersData> LayersData {
		get {
			return mLayersData;
		}
	}
	
	//TODO
	List<LevelLayersData> mHeadersData = new List<LevelLayersData>();
	public List<LevelLayersData> HeadersData {
		get {
			return mHeadersData;
		}
	}	
	
	
	bool mIsSaving = false;
	
	KeyCode mSaveKey = KeyCode.KeypadPeriod;
	KeyCode mLoadKey = KeyCode.Keypad0;
	
	string mLevelLayersFolderName = "LevelLayersData";
	
	//TODO Save the header, and have the Layers saving be based on it
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
	
	public static EditorSave instance = null;
	void Awake() {
		if (instance) {
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			instance = this;		
		}
	}
	
	void CreateFolders () {
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
		//TODO Current loading layers for simplicity and speed. Switch to headers
		
		var layersPathList = Directory.GetFiles (LayersFilePath ());
		foreach (string path in layersPathList) {
			mLayersData.Add (LoadLayerFromDirectPath (path, false));
		}
	}
	
	void Start() {
		CreateFolders ();
		SetComputerHotkeys (); 
		
		LoadLayerHeaders ();
	}
	
	
	void Save(int aSlot) {
		//TODO :(
		#if !UNITY_WP8
		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream file = File.Create(LayersFilePath() + aSlot.ToString() + ".txt");
		binaryFormatter.Serialize(file, LevelLayers.instance.GenerateData ());
		file.Close ();
		#endif
	}
	
	LevelLayersData LoadLayerFromDirectPath (string directPath, bool updateInstance = true) {
		//TODO :(
		#if !UNITY_WP8
		if (File.Exists (directPath)) {
			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream file = File.Open (directPath, FileMode.Open);
			LevelLayersData data = (LevelLayersData)binaryFormatter.Deserialize (file);
			file.Close ();
			
			if(updateInstance) {
				LevelLayers.instance.SetData (data);
				
				//TODO Bug, needs to get mSelect order from headerData. Add this
				LevelHeader.instance.mSelectOrder = 0;
			}
			
			return data;
		}
		
		#endif
		
		return null;
	}	
	
	LevelLayersData LoadLayerFromSlot(int aSlot) {
		string directPath = FilePath () + aSlot.ToString () + ".txt";
		
		return LoadLayerFromDirectPath (directPath, false);
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
			LoadLayerFromSlot(mSlotList[aKeyCode]);
		}
	}
}
