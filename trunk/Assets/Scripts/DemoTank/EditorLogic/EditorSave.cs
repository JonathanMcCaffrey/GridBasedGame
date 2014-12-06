using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class EditorSave : MonoBehaviour, IKeyListener {
	List<KeyCode> mSlotList = new List<KeyCode>();
	
	bool mIsSaving = false;
	
	KeyCode mSaveKey = KeyCode.KeypadPeriod;
	KeyCode mLoadKey = KeyCode.Keypad0;
	
	string FilePath() { 
		return Application.persistentDataPath + "/";
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
	
	void Start() {
		mSlotList.Add (KeyCode.Keypad1); 
		mSlotList.Add (KeyCode.Keypad2);
		mSlotList.Add (KeyCode.Keypad3);
		mSlotList.Add (KeyCode.Keypad4);
		mSlotList.Add (KeyCode.Keypad5);
		mSlotList.Add (KeyCode.Keypad6);
		mSlotList.Add (KeyCode.Keypad7);
		mSlotList.Add (KeyCode.Keypad8);
		mSlotList.Add (KeyCode.Keypad9);
		
		Keys.AddListener (this);
	}
	
	void Save(KeyCode aSlot) {
		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream file = File.Create(FilePath() + aSlot.ToString() + ".txt");
		binaryFormatter.Serialize(file, LevelLayers.instance.GenerateData ());
		file.Close ();
	}


	
	void Load(KeyCode aSlot) {
		
		if (File.Exists (FilePath() + aSlot.ToString () + ".txt")) {
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream file = File.Open(FilePath() + aSlot.ToString () + ".txt", FileMode.Open);
			LevelLayersData data = (LevelLayersData)binaryFormatter.Deserialize(file);
			file.Close();

			LevelLayers.instance.SetData(data);
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
		
		if(!mSlotList.Contains(aKeyCode)) {
			return;
		}
		
		if (mIsSaving) {
			Save(aKeyCode);			
		} else {
			Load(aKeyCode);
		}
	}
}
