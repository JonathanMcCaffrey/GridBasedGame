using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSelect : MonoBehaviour {
	
	public List<Item> mItemList = new List<Item>();
	
	public GameObject mScrollContainer = null;
	
	private Add mAddItem = null;
	
	int mHighestOrder = 0;
	public int GetHighestOrder {
		get {
			return mHighestOrder;
		}
	}
	
	
	
	public static LevelSelect instance = null;
	void Awake() {
		if (instance) {
			Destroy (gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
	}
	
	void SortItemList () {
		mItemList.Sort (delegate (Item left, Item right) {
			if (left.mOrder == right.mOrder) {
				right.mOrder++;
			}
			
			return left.mOrder.CompareTo(right.mOrder);
		});
	}
	
	void RecheckHighestOrder () {
		foreach (Item item in mItemList) {
			if(item.mOrder > mHighestOrder) {
				mHighestOrder = item.mOrder;
			}
		}
	}
	
	void RefreshPositions () {
		int itemIndex = 0;
		foreach (Item item in mItemList) {
			item.gameObject.transform.localPosition = new Vector3 (150 * itemIndex, 0, 0);
			itemIndex++;
		}
		
		mAddItem.gameObject.transform.localPosition = new Vector3 (150 * itemIndex, 0, 0);
	}
	
	public void Refresh() {
		SortItemList ();
		RecheckHighestOrder ();
		RefreshPositions ();
	}
	
	void Start() {
		var itemList = mScrollContainer.GetComponentsInChildren<Item> ();
		foreach (Item item in itemList) {
			mItemList.Add(item);
		}

		
		mAddItem = mScrollContainer.GetComponentInChildren<Add> ();

		Refresh ();
	}
}
