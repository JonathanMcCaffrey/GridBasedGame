using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonLayer : MonoBehaviour {

	public static ButtonLayer instance = null;
	void Awake() {
		if (instance) {
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
	}

	private List<Tile> mTileList = new List<Tile>();

	void SetStartingButtons () {
		foreach (Tile tile in GetComponentsInChildren<Tile> ()) {
			mTileList.Add (tile);
			Editor.instance.mShortCutList [tile.mHotKey] = tile.gameObject;
		}
	}
	
	void Start () {
		SetStartingButtons ();
	}
}
