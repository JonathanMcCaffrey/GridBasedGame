using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSelect : MonoBehaviour {
	
	public List<Item> mItemList = new List<Item>();
	
	public GameObject mScrollContainer = null;
	
	private Add mAddItem = null;
	
	
	public static LevelSelect instance = null;
	void Awake() {
		if (instance) {
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
	}
	
	void Start() {
		var itemList = mScrollContainer.GetComponentsInChildren<Item> ();
		foreach (Item item in itemList) {
			mItemList.Add(item);
		}

		mAddItem = mScrollContainer.GetComponentInChildren<Add> ();

	}


	void Update() {

	}

}
