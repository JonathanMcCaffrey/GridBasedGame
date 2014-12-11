using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class EditorSave : MonoBehaviour, IKeyListener {
	Dictionary<KeyCode, int> mSlotList = new Dictionary<KeyCode, int>();
	
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

	void Start() {
		CreateFolders ();
		SetComputerHotkeys (); 
	}
	

	void Save(int aSlot) {
		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream file = File.Create(LayersFilePath() + aSlot.ToString() + ".txt");
		binaryFormatter.Serialize(file, LevelLayers.instance.GenerateData ());
		file.Close ();
		
		print (FilePath () + aSlot.ToString () + ".txt");
	}
	
	
	void Load(int aSlot) {
		if (File.Exists (FilePath() + aSlot.ToString () + ".txt")) {
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream file = File.Open(LayersFilePath() + aSlot.ToString () + ".txt", FileMode.Open);
			LevelLayersData data = (LevelLayersData)binaryFormatter.Deserialize(file);
			file.Close();
			
			LevelLayers.instance.SetData(data);
			
			LevelHeader.instance.mSelectOrder = aSlot;
		}
		
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
			Load(mSlotList[aKeyCode]);
		}
	}
}
