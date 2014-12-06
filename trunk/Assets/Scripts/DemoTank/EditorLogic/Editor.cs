using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Editor : MonoBehaviour {
	
	public GameObject mSelectedPrefab = null;
	
	public bool mIsOn = true;
	
	public LevelLayers mLevelLayers = null;
	
	public Dictionary<KeyCode,GameObject> mShortCutList = new Dictionary<KeyCode, GameObject>();
	public Dictionary<KeyCode, bool> mKeyIsDown = new Dictionary<KeyCode, bool>();
	
	
	void CheckForEditorToggle () {
		if (Input.GetKeyUp (KeyCode.Space)) {
			mIsOn = !mIsOn;
		}
	}
	
	void CheckForKeySelection () {
		foreach (var code in mShortCutList) {
			if (Input.GetKeyUp (code.Key) && mKeyIsDown [code.Key]) {
				mSelectedPrefab = code.Value;
			}
		}
	}
	
	void UpdateKeys () {
		foreach (var code in mShortCutList) {
			if (Input.GetKeyUp (code.Key)) {
				mKeyIsDown [code.Key] = false;
			}
			if (Input.GetKeyDown (code.Key)) {
				mKeyIsDown [code.Key] = true;
			}
		}
	}
	
	void CheckForMouseTileCreation () {
		if (mIsOn && mSelectedPrefab) {
			if (Input.GetMouseButtonDown (0)) {
				GameObject temp = GameObject.Instantiate (mSelectedPrefab, new Vector3 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y, 0), Quaternion.identity) as GameObject;
				if (mSelectedPrefab.GetComponent<Tile> ().mCollidableType == Tile.CollidableType.Floor) {
					temp.transform.parent =  mLevelLayers.mFloorLayer.transform;
				}
				else
				if (mSelectedPrefab.GetComponent<Tile> ().mCollidableType == Tile.CollidableType.Wall) {
					temp.transform.parent = mLevelLayers.mWallLayer.transform;
				}
			}
		}
	}
	
	void Update () {
		CheckForEditorToggle ();
		CheckForKeySelection ();
		UpdateKeys ();
		
		
		CheckForMouseTileCreation ();
	}
}
