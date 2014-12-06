using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonLayer : MonoBehaviour {
	
	private Editor mEditor = null;
	private List<Tile> mTileList = new List<Tile>();

	void SetStartingButtons () {
		foreach (Tile tile in GetComponentsInChildren<Tile> ()) {
			mTileList.Add (tile);
			mEditor.mShortCutList [tile.mHotKey] = tile.gameObject;
		}
	}
	
	void Start () {
		mEditor = GetComponentInParent<Editor> ();
		
		SetStartingButtons ();
	}
	

}
