using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditorControls : MonoBehaviour, IKeyListener {

	public static EditorControls instance = null;
	void Awake() {
		if (instance) {
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
	}

	void Start() {
		Keys.AddListener(this);
	}

	public GameObject mSelectedPrefab = null;
	
	public bool mIsOn = true;

	public Dictionary<KeyCode,GameObject> mShortCutList = new Dictionary<KeyCode, GameObject>();

	void CheckForMouseTileCreation () {
		if (mIsOn && mSelectedPrefab) {
			if (Input.GetMouseButtonDown (0)) {
				//TODO Write a function to swap all GameObjects with matching Prefabs
				GameObject temp = GameObject.Instantiate (mSelectedPrefab, new Vector3 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y, 0), Quaternion.identity) as GameObject;
				if (mSelectedPrefab.GetComponent<Tile> ().mCollidableType == Tile.CollidableType.Floor) {
					temp.transform.parent =  LevelLayers.instance.mFloorLayer.transform;
				} else if (mSelectedPrefab.GetComponent<Tile> ().mCollidableType == Tile.CollidableType.Wall) {
					temp.transform.parent = LevelLayers.instance.mWallLayer.transform;
				}

				temp.GetComponent<Tile>().mEditorType = Tile.EditorType.Tile;
			}
		}
	}

	void SelectEditorToggle (KeyCode aKeyCode) {
		if (KeyCode.Space == aKeyCode) {
			mIsOn = !mIsOn;
		}
	}
	
	void SelectNewTile (KeyCode aKeyCode) {
		if (mShortCutList.ContainsKey (aKeyCode)) {
			mSelectedPrefab = mShortCutList [aKeyCode];
		}
	}

	public void OnKeyPressed (KeyCode aKeyCode) {
		SelectEditorToggle (aKeyCode);
		SelectNewTile (aKeyCode);
	}

	void OnGUI() {
		if (Input.GetMouseButtonDown (0)) {
			CheckForMouseTileCreation ();
		}
	}
}
