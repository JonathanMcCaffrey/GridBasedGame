using UnityEngine;
using System.Collections;

public class Add : MonoBehaviour {
	public static void AddItemPanelWithRefresh (int order = 0) {
		if (LevelSelect.instance.mDefaultItem) {
			AddItemPanel (order);
			LevelSelect.instance.Refresh ();
		}
	}
	
	public static void AddItemPanel (int order = 0) {
		if (LevelSelect.instance.mDefaultItem) {
			var tempItem = (GameObject.Instantiate (LevelSelect.instance.mDefaultItem) as GameObject).GetComponent<Item> ();
			LevelSelect.instance.mItemList.Add (tempItem);
			if(order == 0) {
				tempItem.mOrder = LevelSelect.instance.GetHighestOrder + 1;
			} else {
				tempItem.mOrder = order;
			}
			tempItem.gameObject.transform.parent = LevelSelect.instance.mScrollContainer.transform;
			tempItem.gameObject.transform.localScale = Vector3.one;
		}
	}
	
	
	
	
	public void onSelected() {
		AddItemPanelWithRefresh ();
	}
}
